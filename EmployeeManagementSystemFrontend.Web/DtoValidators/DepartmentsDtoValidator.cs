
using EmployeeManagementSystemFrontend.Web.Dtos;
using FluentValidation;

namespace EmployeeManagementSystemFrontend.Web.DtoValidators
{
    public class CreateDepartmentValidator:AbstractValidator<CreateDepartmentDto>
    {
        public CreateDepartmentValidator()
        {
            RuleFor(d => d.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(d => d.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
        }
    }

    public class UpdateDepartmentValidator : AbstractValidator<UpdateDepartmentDto>
        {
            public UpdateDepartmentValidator()
            {
                RuleFor(d => d.Id)
                    .NotNull().WithMessage("Department Id can not be null!")
                    .GreaterThan(0).WithMessage("Department Id must be greater than 0!");

                RuleFor(d => d.Name)
                    .NotEmpty().WithMessage("Name is required.")
                    .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

                RuleFor(d => d.Description)
                    .NotEmpty().WithMessage("Description is required.")
                    .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
            }
        }
}