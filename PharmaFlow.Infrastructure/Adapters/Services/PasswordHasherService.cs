namespace PharmaFlow.Infrastructure.Adapters.Services;

public class PasswordHasherService
{
    public string Hash(string password)
    {
        return password;
    }

    public bool Verify(string password, string hash)
    {
        return password == hash;
    }
}