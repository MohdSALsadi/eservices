using System.Security.Cryptography;
using System.Text;

namespace Pattern_of_life.Authentication
{

    public static class EncryptionHelper
    {
        private const string EncryptionKey = "APPLICATION";

        public static string Encrypt(string plainText)
        {
            StringBuilder encryptedText = new StringBuilder();
            for (int i = 0; i < plainText.Length; i++)
                encryptedText.Append((char)(plainText[i] ^ EncryptionKey[i % EncryptionKey.Length]));
            return encryptedText.ToString();
        }

        public static string Decrypt(string encryptedText)
        {
            StringBuilder plainText = new StringBuilder();
            for (int i = 0; i < encryptedText.Length; i++)
                plainText.Append((char)(encryptedText[i] ^ EncryptionKey[i % EncryptionKey.Length]));
            return plainText.ToString();
        }

    }



}
