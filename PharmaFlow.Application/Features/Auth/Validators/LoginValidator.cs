namespace PharmaFlow.Application.Features.Auth.Validators;

public class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(x => x.Correo)
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}