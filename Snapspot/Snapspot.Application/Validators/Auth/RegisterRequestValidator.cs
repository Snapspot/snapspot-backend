using FluentValidation;
using Snapspot.Application.Models.Requests.Auth;
using Snapspot.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snapspot.Application.Validators.Auth
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator(IUserRepository userRepository)
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .Length(0, 50).WithMessage("Email must be less than 50 characters.")
                .MustAsync(async (userName, cancellation) => !await userRepository.ExistsByEmailAsync(userName));

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm Password is required.")
                .Equal(x => x.Password).WithMessage("Confirm Password must match Password.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone Number is required.")
                .Length(0, 20).WithMessage("Phone Number must be less than 20 characters.");

            RuleFor(x => x.Dob)
                .NotEmpty().WithMessage("Date of Birth is required.")
                .LessThan(DateTime.UtcNow).WithMessage("Date of Birth must be in the past.");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required.")
                .Must(role =>
                    role != null &&
                    (role.Equals("Admin", StringComparison.OrdinalIgnoreCase) ||
                     role.Equals("User", StringComparison.OrdinalIgnoreCase) ||
                     role.Equals("ThirdParty", StringComparison.OrdinalIgnoreCase)))
                .WithMessage("Role must be 'Admin', 'User' or 'ThirdParty'.");
        }
    }
}
