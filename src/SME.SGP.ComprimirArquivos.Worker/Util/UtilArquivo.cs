using System;
using System.IO;
using SME.SGP.Infra;

namespace SME.SGP.ComprimirArquivos.Worker
{
    public static class UtilArquivo
    {
        public static string ObterDiretorioBase()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
        }
        
        public static string ObterDiretorioCompletoArquivos()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ArquivoConstantes.PastaArquivos);
        }
        
        public static string ObterDiretorioCompletoTemporario()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ArquivoConstantes.PastaTemp);
        }
    }
}