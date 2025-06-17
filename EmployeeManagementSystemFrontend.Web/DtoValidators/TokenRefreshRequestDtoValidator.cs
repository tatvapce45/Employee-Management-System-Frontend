using EmployeeManagementSystemFrontend.Web.Dtos;
using FluentValidation;

namespace EmployeeManagementSystemFrontend.Web.DtoValidators
{
    public class TokenRefreshRequestDtoValidator:AbstractValidator<TokenRefreshRequestDto>
    {
        public TokenRefreshRequestDtoValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required.");
        }
    }
}