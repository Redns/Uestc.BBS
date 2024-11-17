using System.Security.Cryptography;
using System.Text;

namespace Uestc.BBS.Core.Helpers
{
    public static class DesHelper
    {
        public static string SecretNormalization(string? secret) => string.IsNullOrEmpty(secret) ? string.Empty : secret.Length switch
        {
            < 16 => secret.PadRight(16, '0'),
            16 => secret,
            _ => secret[..16]
        };

        public static string Encrypt(this string content, string? secret = null)
        {
            if (string.IsNullOrEmpty(secret = SecretNormalization(secret)))
            {
                return content;
            }

            var bKey = new byte[32];
            var bVector = new byte[16];
            var plainBytes = Encoding.UTF8.GetBytes(content);
            Array.Copy(Encoding.UTF8.GetBytes(secret.PadRight(bKey.Length)), bKey, bKey.Length);
            Array.Copy(Encoding.UTF8.GetBytes(secret.PadRight(bVector.Length)), bVector, bVector.Length);

            var aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.BlockSize = 128;
            aes.Padding = PaddingMode.PKCS7;

            using var memory = new MemoryStream();
            using var encryptor = new CryptoStream(memory, aes.CreateEncryptor(bKey, bVector), CryptoStreamMode.Write);
            encryptor.Write(plainBytes, 0, plainBytes.Length);
            encryptor.FlushFinalBlock();

            return Convert.ToBase64String(memory.ToArray());
        }

        public static string Decrypt(this string content, string? secret = null)
        {
            if (string.IsNullOrEmpty(content) || string.IsNullOrEmpty(secret = SecretNormalization(secret)))
            {
                return content;
            }

            var bKey = new byte[32];
            var bVector = new byte[16];
            var encryptedBytes = Convert.FromBase64String(content);
            Array.Copy(Encoding.UTF8.GetBytes(secret.PadRight(bKey.Length)), bKey, bKey.Length);
            Array.Copy(Encoding.UTF8.GetBytes(secret.PadRight(bVector.Length)), bVector, bVector.Length);

            var aes = Aes.Create();
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var memory = new MemoryStream(encryptedBytes);
            using var Decryptor = new CryptoStream(memory, aes.CreateDecryptor(bKey, bVector), CryptoStreamMode.Read);
            using var originalMemory = new MemoryStream();

            var readBytes = 0;
            var Buffer = new byte[1024];
            while ((readBytes = Decryptor.Read(Buffer, 0, Buffer.Length)) > 0)
            {
                originalMemory.Write(Buffer, 0, readBytes);
            }
            return Encoding.UTF8.GetString(originalMemory.ToArray());
        }
    }
}
