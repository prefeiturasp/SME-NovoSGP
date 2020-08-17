using System;
using System.Security.Cryptography;
using System.Text;

namespace SME.SGP.Infra.Utilitarios
{
    public static class UtilCriptografia
    {
        public static string ConverterBase64(string entrada)
        {
            byte[] toEncodeAsBytes = Encoding.ASCII.GetBytes(entrada);

            return Convert.ToBase64String(toEncodeAsBytes);
        }

        public static string DesconverterBase64(string entrada)
        {
            byte[] encodedDataAsBytes = Convert.FromBase64String(entrada);

            return Encoding.ASCII.GetString(encodedDataAsBytes);
        }
        public static string GerarHashSha1(string valor)
        {
            using (var sha1 = SHA1.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(valor);

                byte[] hashBytes = sha1.ComputeHash(bytes);

                return HexStringFromBytes(hashBytes);
            }
        }
        public static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }
    }
}
