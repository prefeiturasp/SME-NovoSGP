using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.Relatorios.Devolutivas
{
    public class SolicitaRelatorioDevolutivasCommandHandler : IRequestHandler<SolicitaRelatorioDevolutivasCommand, Guid>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public SolicitaRelatorioDevolutivasCommandHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }
        public async Task<Guid> Handle(SolicitaRelatorioDevolutivasCommand request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoServidorRelatorios");

            var filtro = new FiltroRelatorioDevolutivasSincrono()
            {
                DevolutivaId = request.DevolutivaId,
                UsuarioNome = request.UsuarioNome,
                UsuarioRF = request.UsuarioRF,
                UeId = request.UeId,
                TurmaId = request.TurmaId
            };

            var resposta = await httpClient.PostAsync($"api/v1/relatorios/sincronos/devolutivas", new StringContent(JsonConvert.SerializeObject(filtro), Encoding.UTF8, "application/json-patch+json"));

            if (!resposta.IsSuccessStatusCode && resposta.StatusCode == HttpStatusCode.NoContent)
                throw new NegocioException("Não foi possível Obter o relatório de devolutivas.");

            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Guid>(json);
        }
    }
}
