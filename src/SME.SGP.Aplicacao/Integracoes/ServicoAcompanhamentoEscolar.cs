using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Integracoes
{
    public class ServicoAcompanhamentoEscolar : IServicoAcompanhamentoEscolar
    {
        private readonly IConfiguration configuration;
        private readonly HttpClient httpClient;

        public ServicoAcompanhamentoEscolar(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task AlterarComunicado(ComunicadoInserirAeDto comunicado, long id)
        {
            CriarParametrosEHEader(comunicado, out string parametros, out StringContent input);

            using (var resposta = await httpClient.PutAsync($"v1/notificacao/{id}", input))
            {
                if (!resposta.IsSuccessStatusCode)
                    throw new NegocioException("Não foi possível salvar os dados no aplicativo do aluno");
            }
        }

        public async Task CriarComunicado(ComunicadoInserirAeDto comunicado)
        {
            CriarParametrosEHEader(comunicado, out string parametros, out StringContent input);

            using (var resposta = await httpClient.PostAsync("v1/notificacao", input))
            {
                if (!resposta.IsSuccessStatusCode)
                    throw new NegocioException("Não foi possível alterar os dados no aplicativo do aluno");
            }
        }

        public async Task ExcluirComunicado(long[] ids)
        {
            httpClient.DefaultRequestHeaders.Clear();
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(httpClient.BaseAddress.ToString() + "v1/notificacao"),
                Method = HttpMethod.Delete,
                Content = new StringContent(JsonConvert.SerializeObject(ids), Encoding.UTF8, "application/json"),
            };
            request.Headers.Add("x-integration-key", configuration.GetSection("AE_ChaveIntegracao").Value);


            using (var resposta = await httpClient.SendAsync(request))
            {
                if (!resposta.IsSuccessStatusCode)
                    throw new NegocioException("Não foi possível excluir os dados no aplicativo do aluno");
            }
        }

        private void CriarParametrosEHEader(ComunicadoInserirAeDto comunicado, out string parametros, out StringContent input)
        {
            httpClient.DefaultRequestHeaders.Clear();
            parametros = JsonConvert.SerializeObject(comunicado);
            input = new StringContent(parametros, Encoding.UTF8, "application/json");
            input.Headers.Add("x-integration-key", configuration.GetSection("AE_ChaveIntegracao").Value);
        }
    }
}