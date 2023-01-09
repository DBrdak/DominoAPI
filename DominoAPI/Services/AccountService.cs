using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using AutoMapper;
using DominoAPI.Authorization;
using DominoAPI.Entities;
using DominoAPI.Entities.Accounts;
using DominoAPI.Exceptions;
using DominoAPI.Models;
using DominoAPI.Models.AccountModels;
using DominoAPI.Models.AccountModels.Display;
using DominoAPI.Models.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UtilityLibrary;

namespace DominoAPI.Services
{
    public interface IAccountService
    {
        Task RegisterUser(RegisterUserDto dto);

        Task<string> LoginUser(LoginUserDto dto);

        Task<PagedResult<DisplayUserDto>> GetAllUsers(UserQueryParams query);

        Task UpdateUserAccount(UpdateUserDto dto);

        Task ManageUser(UpdateExternalUserDto dto, int userId);

        Task DeleteUser(int userId);
    }

    public class AccountService : IAccountService
    {
        private readonly DominoDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;

        public AccountService(DominoDbContext dbContext, IMapper mapper, IPasswordHasher<User> passwordHasher,
            AuthenticationSettings authenticationSettings, IAuthorizationService authorizationService,
            IUserContextService userContextService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
        }

        public async Task RegisterUser(RegisterUserDto dto)
        {
            dto.Email = dto.Email.ToLower();

            var newUser = _mapper.Map<User>(dto);

            newUser.PasswordHash = _passwordHasher.HashPassword(newUser, dto.Password);

            await _dbContext.Users.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<string> LoginUser(LoginUserDto dto)
        {
            var user = await _dbContext.Users
                .Include(u => u.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user is null)
            {
                throw new BadRequestException("Invalid email or password");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

            if (result is PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid email or password");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Role.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenHandler;
        }

        public async Task<PagedResult<DisplayUserDto>> GetAllUsers(UserQueryParams query)
        {
            var baseUsers = await _dbContext.Users
                .Where(u => u.Email.Contains(query.SearchPhrase)
                            || query.SearchPhrase == null)
                .AsNoTracking()
                .ToListAsync();

            if (!baseUsers.Any())
            {
                throw new NotFoundException("Content not found");
            }

            baseUsers = baseUsers.Sort(query.SortBy, query.SortDirection.ToString()).ToList();

            var users = baseUsers.GetPage(query.PageSize, query.PageId);

            var dto = _mapper.Map<IEnumerable<DisplayUserDto>>(users);

            var result = new PagedResult<DisplayUserDto>
                (dto, baseUsers.Count, query.PageSize, query.PageId);

            return result;
        }

        public async Task UpdateUserAccount(UpdateUserDto dto)
        {
            var userId = _userContextService.GetUserId;

            var user = _userContextService.User;

            var userToUpdate = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            var authorizationResult = await _authorizationService.AuthorizeAsync
                (user, userToUpdate, new ResourceOperationRequirement(ResourceOperation.Update));

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("Not permission for this action");
            }

            dto.MapTo(userToUpdate);

            if (dto.Password is not null)
            {
                var verifyPassword = _passwordHasher.VerifyHashedPassword
                    (userToUpdate, userToUpdate.PasswordHash, dto.PreviousPassword);

                if (verifyPassword is PasswordVerificationResult.Failed)
                {
                    throw new BadRequestException("Wrong password");
                }

                userToUpdate.PasswordHash = _passwordHasher.HashPassword(userToUpdate, dto.Password);
            }

            _dbContext.Users.Update(userToUpdate);
            await _dbContext.SaveChangesAsync();
        }

        public async Task ManageUser(UpdateExternalUserDto dto, int userId)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            var role = await _dbContext.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == dto.RoleId);

            if (role == null)
            {
                throw new NotFoundException("Role not found");
            }

            user.RoleId = dto.RoleId;

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteUser(int userId)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}