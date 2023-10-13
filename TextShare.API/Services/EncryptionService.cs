namespace TextShare.API.Services;

public interface IEncryptionService
{
    string HashPassword(string password);
    bool VerifyPassword(string? password, string hashedPassword);
}

public class EncryptionService : IEncryptionService
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string? password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}