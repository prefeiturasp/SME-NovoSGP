using SME.SGP.Dominio;
using System;
using System.Net;

namespace SME.SGP.Infra.Extensoes
{
    public static class WebClientExtensao
    {
        public static bool TryDownloadData(this WebClient client, string nomeArquivo,  
                                           out byte[] arquivo, 
                                           out Exception excecao)
        {
            arquivo = null;
            excecao = null;
            try
            {
                arquivo = client.DownloadData(new Uri(nomeArquivo));
                return true;
            }
            catch (Exception e)
            {
                excecao = e;
                return false;
            }
        }
    }
}
