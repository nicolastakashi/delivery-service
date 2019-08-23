using System;
using System.Security.Cryptography;
using System.Text;

namespace DeliveryService.Infra.Security
{
    public struct SecurityString
    {
        public static string Hash(string value)
        {
            var data = Encoding.ASCII.GetBytes(value);

            using (var sHa256Managed = new SHA256Managed())
            {
                var crypto = sHa256Managed.ComputeHash(data);
                var hash = new StringBuilder();

                for (var i = 0; i < crypto.Length; i++)
                {
                    hash.Append(crypto[i].ToString("x2"));
                }

                return hash.ToString();
            }
        }

        public static string GetRandom()
        {
            const string caracteresValidos = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var chars = new char[8];

            for (var i = 0; i < 8; i++)
            {
                chars[i] = caracteresValidos[random.Next(0, caracteresValidos.Length)];
            }

            return chars.ToString();
        }
    }
}
