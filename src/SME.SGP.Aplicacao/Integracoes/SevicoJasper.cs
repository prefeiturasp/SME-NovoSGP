using MediatR;
using SME.SGP.Dominio.Enumerados;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Integracoes
{
    public class SevicoJasper : ISevicoJasper
    {
        private readonly HttpClient httpClient;
        private readonly JasperCookieHandler jasperCookieHandler;
        private readonly IMediator mediator;

        public SevicoJasper(HttpClient httpClient, JasperCookieHandler jasperCookieHandler, IMediator mediator)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.jasperCookieHandler = jasperCookieHandler ?? throw new ArgumentNullException(nameof(jasperCookieHandler));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<byte[]> DownloadRelatorio(Guid exportID, Guid requestId, string jSessionId)
        {
            if (!string.IsNullOrWhiteSpace(jSessionId))
                jasperCookieHandler.CookieContainer.Add(httpClient.BaseAddress, new System.Net.Cookie("JSESSIONID", jSessionId));

            var resposta = await httpClient.GetAsync($"/jasperserver/rest_v2/reportExecutions/{requestId}/exports/{exportID}/outputResource");

            if (resposta.IsSuccessStatusCode)
                return await resposta.Content.ReadAsByteArrayAsync();

            var strBuilderMensagemErro = new StringBuilder();

            strBuilderMensagemErro.AppendLine($"DOWNLOAD RELATÓRIO ERRO STATUS CODE: {resposta.StatusCode}");
            strBuilderMensagemErro.AppendLine($"DOWNLOAD RELATÓRIO ERRO: {await resposta.Content.ReadAsStringAsync()}");

            await mediator.Send(new SalvarLogViaRabbitCommand(strBuilderMensagemErro.ToString(), LogNivel.Negocio, LogContexto.Relatorios, string.Empty));

            return null;
        }
    }
}
