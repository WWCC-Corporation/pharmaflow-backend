using PharmaFlow.Application.Auth.DTOs;

namespace PharmaFlow.Application.Auth.Validators;

public class LoginValidator
{
    public bool Validate(LoginRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Correo))
            return false;

        if (string.IsNullOrWhiteSpace(request.Password))
            return false;

        return true;
    }
}