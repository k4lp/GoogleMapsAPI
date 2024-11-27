namespace GoogleMapsAPI.Services.HashingAndEncryptionService.EncryptionService
{
    public interface IEncryptionService
    {
        string Encrypt(string plainText);
        string Decrypt(string cipherText);
    }
}
