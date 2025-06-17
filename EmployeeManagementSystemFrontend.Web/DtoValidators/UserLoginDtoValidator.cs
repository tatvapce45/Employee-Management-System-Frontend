using EmployeeManagementSystemFrontend.Web.Dtos;
using FluentValidation;

namespace EmployeeManagementSystemFrontend.Web.DtoValidators
{
    public class UserLoginDtoValidator:AbstractValidator<UserLoginDto>
    {
        public UserLoginDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email field must not be null!")
                .EmailAddress().WithMessage("Please enter valid email address!")
                .MaximumLength(50).WithMessage("Email field must not be longer than 50 characters!");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("New Password field must not be null!")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$")
                .WithMessage("Password must contain at least 1 uppercase, 1 lowercase, 1 number, and 1 special character.");

        }
    }
}