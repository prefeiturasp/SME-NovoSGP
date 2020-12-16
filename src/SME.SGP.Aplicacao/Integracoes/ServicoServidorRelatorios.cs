using Sentry;
using SME.SGP.Dominio;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Integracoes
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

            var mensagemErro = await resposta.Content.ReadAsStringAsync();
            
            SentrySdk.AddBreadcrumb($"Erro ao fazer o download");
            SentrySdk.AddBreadcrumb($"Base Address: {httpClient.BaseAddress}");
            SentrySdk.AddBreadcrumb($"Status code: {resposta.StatusCode}");
            SentrySdk.AddBreadcrumb($"Mensagem Erro: {mensagemErro}");
            throw new NegocioException("Não foi possível realizar o download do relatório.");
        }
    }
}
