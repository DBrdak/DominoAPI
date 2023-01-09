using DominoAPI.Entities;
using DominoAPI.Exceptions;
using FluentValidation;
using System.Text.RegularExpressions;
using UtilityLibrary;

namespace DominoAPI.Models.AccountModels
{
    public class AccountValidator : AbstractValidator<RegisterUserDto>
    {
        public AccountValidator(DominoDbContext dbContext)
        {
            RuleFor(p => p.Email)
                .Custom((value, context) =>
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
                });
            RuleFor(p => p.Password)
                .Custom((value, context) =>
                {
                    if (!value.ValidatePassword())
                    {
                        context.AddFailure("Password", "Password is too weak");
                    }
                });
            RuleFor(p => p.PasswordConfirm)
                .Equal(p => p.Password)
                .WithMessage("Password doesn't match");
        }
    }
}