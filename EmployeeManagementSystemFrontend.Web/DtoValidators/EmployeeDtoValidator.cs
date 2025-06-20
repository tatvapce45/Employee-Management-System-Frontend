using EmployeeManagementSystemFrontend.Web.Dtos;
using FluentValidation;

namespace EmployeeManagementSystemFrontend.Web.DtoValidators
{
    public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeDto>
    {
        public CreateEmployeeValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name field must not be null!")
                .MaximumLength(60).WithMessage("Name field must not be longer than 60 characters!");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email field must not be null!")
                .EmailAddress().WithMessage("Please enter valid email address!")
                .MaximumLength(50).WithMessage("Email field must not be longer than 50 characters!");

            RuleFor(x => x.MobileNo)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\d{10}$").WithMessage("Phone number must be exactly 10 digits with no characters.");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender field must not be null!")
                .MaximumLength(10).WithMessage("Gender must be less or equal 10 characters!");

            RuleFor(x => x.Age)
                .NotNull().WithMessage("Employee age can not be null!")
                .GreaterThan(18).WithMessage("Employee age must be greater than 18!");

            RuleFor(x => x.Salary)
                .NotNull().WithMessage("Employee salary can not be null!")
                .GreaterThan(0).WithMessage("Employee salary must be greater than 0!")
                .LessThanOrEqualTo(10000000).WithMessage("Employee salary must be less than or equal to 10,000,000!");

            RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address field must not be null!")
            .MaximumLength(1000).WithMessage("Address field must not be longer than 1000 characters!");

            RuleFor(x => x.Zipcode)
                .NotEmpty().WithMessage("Zipcode field must not be null!")
                .Matches(@"^\d{6}$").WithMessage("Zipcode must be exactly 6 digits with no characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("New Password field must not be null!")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$")
                .WithMessage("Password must contain at least 1 uppercase, 1 lowercase, 1 number, and 1 special character.");

            RuleFor(x => x.CountryId)
                .NotEmpty().WithMessage("Country field must not be null!")
                .GreaterThan(0).WithMessage("Country field must be greater than 0!");

            RuleFor(x => x.StateId)
                .NotEmpty().WithMessage("State field must not be null!")
                .GreaterThan(0).WithMessage("State field must be greater than 0!");

            RuleFor(x => x.CityId)
                .NotEmpty().WithMessage("City field must not be null!")
                .GreaterThan(0).WithMessage("City field must be greater than 0!");

            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage("Role field must not be null!")
                .GreaterThan(0).WithMessage("Role field must be greater than 0!");
        }
    }

    public class UpdateEmployeeValidator : AbstractValidator<UpdateEmployeeDto>
    {
        public UpdateEmployeeValidator()
        {
            RuleFor(x => x.Id)
                .NotNull().WithMessage("Employee Id can not be null!")
                .GreaterThan(0).WithMessage("Employee Id must be greater than 0!");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name field must not be null!")
                .MaximumLength(60).WithMessage("Name field must not be longer than 60 characters!");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email field must not be null!")
                .EmailAddress().WithMessage("Please enter valid email address!")
                .MaximumLength(50).WithMessage("Email field must not be longer than 50 characters!");

            RuleFor(x => x.MobileNo)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\d{10}$").WithMessage("Phone number must be exactly 10 digits with no characters.");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender field must not be null!")
                .MaximumLength(10).WithMessage("Gender must be less or equal 10 characters!");

            RuleFor(x => x.Age)
                .NotNull().WithMessage("Employee age can not be null!")
                .GreaterThan(0).WithMessage("Employee age must be greater than 18!");

            RuleFor(x => x.Salary)
                .NotNull().WithMessage("Employee salary can not be null!")
                .GreaterThan(0).WithMessage("Employee salary must be greater than 0!")
                .LessThanOrEqualTo(10000000).WithMessage("Employee salary must be less than or equal to 10,000,000!");

            RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address field must not be null!")
            .MaximumLength(1000).WithMessage("Address field must not be longer than 1000 characters!");

            RuleFor(x => x.Zipcode)
                .NotEmpty().WithMessage("Zipcode field must not be null!")
                .Matches(@"^\d{6}$").WithMessage("Zipcode must be exactly 6 digits with no characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("New Password field must not be null!")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$")
                .WithMessage("Password must contain at least 1 uppercase, 1 lowercase, 1 number, and 1 special character.");

            RuleFor(x => x.CountryId)
                .NotEmpty().WithMessage("Country field must not be null!")
                .GreaterThan(0).WithMessage("Country field must be greater than 0!");

            RuleFor(x => x.StateId)
                .NotEmpty().WithMessage("State field must not be null!")
                .GreaterThan(0).WithMessage("State field must be greater than 0!");

            RuleFor(x => x.CityId)
                .NotEmpty().WithMessage("City field must not be null!")
                .GreaterThan(0).WithMessage("City field must be greater than 0!");

            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage("Role field must not be null!")
                .GreaterThan(0).WithMessage("Role field must be greater than 0!");
        }
    }
}