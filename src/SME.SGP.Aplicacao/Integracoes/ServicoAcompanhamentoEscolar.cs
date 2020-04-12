using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Integracoes
{
    public class ServicoAcompanhamentoEscolar : IServicoAcompanhamentoEscolar
    {
        private readonly IRepositorioCache cache;
        private readonly HttpClient httpClient;
        private readonly IServicoLog servicoLog;

        public ServicoAcompanhamentoEscolar(HttpClient httpClient, IRepositorioCache cache, IServicoLog servicoLog)
        {
            this.httpClient = httpClient;
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
            this.cache = cache;
        }

        public async Task AlterarComunicado(ComunicadoInserirDto comunicado)
        {
            StringContent parametros = CriarParametrosComunicado(comunicado);
            using (var resposta = await httpClient.PutAsync("v1/notificacao/", parametros))
            {
                if (!resposta.IsSuccessStatusCode)
                    await RegistrarLogSentryAsync(resposta, "alterar comunicado", parametros.ToString());
            }
        }

        public async Task CriarComunicado(ComunicadoInserirDto comunicado)
        {
            StringContent parametros = CriarParametrosComunicado(comunicado);
            using (var resposta = await httpClient.PostAsync("v1/notificacao/", parametros))
            {
                if (!resposta.IsSuccessStatusCode)
                    await RegistrarLogSentryAsync(resposta, "criar comunicado", parametros.ToString());
            }
        }

        public Task ExcluirComunicado(long id)
        {
            throw new NotImplementedException();
        }

        private static StringContent CriarParametrosComunicado(ComunicadoInserirDto comunicado)
        {
            return new StringContent(JsonConvert.SerializeObject(comunicado), Encoding.UTF8, "application/json");
        }

        private async Task RegistrarLogSentryAsync(HttpResponseMessage resposta, string rotina, string parametros)
        {
            if (resposta.StatusCode != HttpStatusCode.NotFound)
            {
                var mensagem = await resposta.Content.ReadAsStringAsync();
                servicoLog.Registrar(new NegocioException($"Ocorreu um erro ao {rotina} no Acompanhamento Escolar, código de erro: {resposta.StatusCode}, mensagem: {mensagem ?? "Sem mensagem"},Parametros:{parametros}, Request: {JsonConvert.SerializeObject(resposta.RequestMessage)}, "));
            }
        }
    }
}