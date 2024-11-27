namespace GoogleMapsAPI.Services.HashingAndEncryptionService.HashingService
{
    public interface IHashingService
    {
        string GenerateHash(string RawInput);
        bool VerifyHash(string RawInput, string HashValue);
    }
}
