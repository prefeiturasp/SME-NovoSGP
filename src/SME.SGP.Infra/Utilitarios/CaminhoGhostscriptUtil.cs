using System;
using System.IO;
using System.Runtime.InteropServices;

namespace SME.SGP.Infra.Utilitarios
{
    public class CaminhoGhostscriptUtil
    {
        public string ObterCaminhoGhostscript()
        {
            // Verifica se esta no Windows
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // No Windows, o executável é gswin64c.exe (ou gswin32c.exe para 32-bit)
                // A função FindExecutableInPath vai procurar por ele no PATH do sistema
                return FindExecutableInPath("gswin64c.exe");
            }
            // Assume Linux/macOS
            else
            {
                // No Linux/macOS, o executável é gs
                return FindExecutableInPath("gs");
            }
        }
        private string FindExecutableInPath(string executableName)
        {
            var pathEnv = Environment.GetEnvironmentVariable("PATH");
            if (string.IsNullOrEmpty(pathEnv))
            {
                return null;
            }

            var paths = pathEnv.Split(Path.PathSeparator);
            foreach (var path in paths)
            {
                var fullPath = Path.Combine(path, executableName);
                if (File.Exists(fullPath))
                {
                    return fullPath;
                }
            }
            return null;
        }
    }
}
