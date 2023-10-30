namespace UserApi.Helpers;

public interface IPasswordHasher
{
    (byte[] hash, byte[] salt) HashPassword(string password);
    bool ValidatePassword(string password, byte[] hash, byte[] salt);
}
