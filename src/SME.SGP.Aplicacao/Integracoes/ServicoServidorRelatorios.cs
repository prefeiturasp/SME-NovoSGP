using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Integracoes
{
    public class ServicoServidorRelatorios : IServicoServidorRelatorios
    {
        private readonly HttpClient httpClient;
        private readonly IMediator mediator;

        public ServicoServidorRelatorios(HttpClient httpClient, IMediator mediator)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<byte[]> DownloadRelatorio(Guid correlacaoId)
        {
            var resposta = await httpClient.GetAsync($"api/v1/downloads/sgp/pdf/{correlacaoId}");

            if (resposta.IsSuccessStatusCode)
                return await resposta.Content.ReadAsByteArrayAsync();

            var mensagemErro = await resposta.Content.ReadAsStringAsync();

            var strBuilderMensagemErro = new StringBuilder();

            strBuilderMensagemErro.AppendLine($"Erro ao fazer o download");
            strBuilderMensagemErro.AppendLine($"Base Address: {httpClient.BaseAddress}");
            strBuilderMensagemErro.AppendLine($"Status code: {resposta.StatusCode}");
            strBuilderMensagemErro.AppendLine($"Mensagem Erro: {mensagemErro}");

            await mediator.Send(new SalvarLogViaRabbitCommand(strBuilderMensagemErro.ToString(), LogNivel.Negocio, LogContexto.Relatorios, string.Empty));

            throw new NegocioException("Não foi possível realizar o download do relatório.");
        }
    }
}
