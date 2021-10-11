using System;
using System.IO;

namespace SME.SGP.Infra
{
    public static class UtilArquivo
    {
        public static string ObterDiretorioBase()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Arquivos");
        }
    }
}