using SME.SGP.Dominio;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ServicoServidorRelatorios : IServicoServidorRelatorios
    {
        private readonly HttpClient httpClient;

        public ServicoServidorRelatorios(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<byte[]> DownloadRelatorio(Guid correlacaoId)
        {
            var resposta = await httpClient.GetAsync($"api/v1/downloads/sgp/pdf/{correlacaoId}");

            if (resposta.IsSuccessStatusCode)
                return await resposta.Content.ReadAsByteArrayAsync();

            throw new NegocioException("Não foi possível realizar o download do relatório.");
        }
    }
}
