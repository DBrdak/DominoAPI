using System.Text.RegularExpressions;
using DominoAPI.Entities;
using FluentValidation;
using UtilityLibrary;

namespace DominoAPI.Models.AccountModels
{
    public class UserUpdateValidator : AbstractValidator<UpdateUserDto>
    {
        public UserUpdateValidator(DominoDbContext dbContext)
        {
            RuleFor(p => p.Email)
                .Custom((value, context) =>
                {
                    if (value != null)
                    {
                        var isEmailInUse = dbContext.Users.Any(e => e.Email.Equals(value));

                        if (isEmailInUse)
                        {
                            context.AddFailure("Email", "This email is already in use");
                        }

                        var emailPattern = new Regex(@"^([a-z0-9]+)[$&+,:;=?@#|'<>.^*()%!-]?([a-z0-9]+)@([a-z]+)\.[a-z]{2,3}$");
                        var isEmailValid = emailPattern.IsMatch(value.ToLower());

                        if (!isEmailValid)
                        {
                            context.AddFailure("Email", "Wrong email format");
                        }
                    }
                });
            RuleFor(p => p.Password)
                .Custom((value, context) =>
                {
                    if (value is not null)
                    {
                        if (!value.ValidatePassword())
                        {
                            context.AddFailure("Password", "Password is too weak");
                        }
                    }
                });
        }
    }
}