namespace CirclesFundMe.Application.Services
{
    public record EncryptionService
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;

        public EncryptionService(string key, string iv)
        {
            if (key == null || iv == null)
                throw new ArgumentNullException("Key and IV cannot be null");

            _key = Convert.FromBase64String(key);
            _iv = Convert.FromBase64String(iv);

            if (_key.Length != 16 && _key.Length != 24 && _key.Length != 32)
                throw new ArgumentException("Key size must be 128, 192, or 256 bits");
            if (_iv.Length != 16)
                throw new ArgumentException("IV size must be 128 bits");
        }

        public string Encrypt(string plainText)
        {
            if (plainText != null)
            {
                using Aes aesAlg = Aes.Create();
                aesAlg.Key = _key;
                aesAlg.IV = _iv;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using MemoryStream msEncrypt = new();
                using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
                using (StreamWriter swEncrypt = new(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                }
                return Convert.ToBase64String(msEncrypt.ToArray());
            }

            throw new ArgumentNullException(nameof(plainText));
        }

        public string Decrypt(string cipherText)
        {
            if (cipherText != null)
            {
                using Aes aesAlg = Aes.Create();
                aesAlg.Key = _key;
                aesAlg.IV = _iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using MemoryStream msDecrypt = new(Convert.FromBase64String(cipherText));
                using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
                using StreamReader srDecrypt = new(csDecrypt);
                return srDecrypt.ReadToEnd();
            }

            throw new ArgumentNullException(nameof(cipherText));
        }
    }
}
