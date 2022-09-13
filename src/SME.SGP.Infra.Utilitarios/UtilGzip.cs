using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace SME.SGP.Infra.Utilitarios
{
    public static class UtilGZip
    {
        public static byte[] Comprimir(String stringValue)
        {
            var bytes = Encoding.UTF8.GetBytes(stringValue);
            using (var inputStream = new MemoryStream(bytes))
            {
                using (var outputStream = new MemoryStream())
                {
                    using (var gzipStream = new GZipStream(outputStream, CompressionMode.Compress))
                    {
                        Copiar(inputStream, gzipStream);
                    }
                    return outputStream.ToArray();
                }
            }
        }

        public static String Descomprimir(byte[] bytes)
        {
            using (var inputStream = new MemoryStream(bytes))
            {
                using (var outputStream = new MemoryStream())
                {
                    using (var gzipStream = new GZipStream(inputStream, CompressionMode.Decompress))
                    {
                        Copiar(gzipStream, outputStream);
                    }

                    return Encoding.UTF8.GetString(outputStream.ToArray());
                }
            }
        }

        private static void Copiar(Stream sourceStream, Stream destinationStream)
        {
            byte[] bytes = new byte[4096];
            int cnt;
            while ((cnt = sourceStream.Read(bytes, 0, bytes.Length)) != 0)
            {
                destinationStream.Write(bytes, 0, cnt);
            }
        }
    }
}