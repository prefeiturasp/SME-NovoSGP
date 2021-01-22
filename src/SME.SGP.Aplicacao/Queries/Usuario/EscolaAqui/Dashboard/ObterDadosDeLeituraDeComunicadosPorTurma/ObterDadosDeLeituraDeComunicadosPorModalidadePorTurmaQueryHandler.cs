using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos.EscolaAqui.DadosDeLeituraDeComunicados;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosDeLeituraDeComunicadosPorModalidadePorTurmaQueryHandler : IRequestHandler<ObterDadosDeLeituraDeComunicadosPorModalidadePorTurmaQuery, IEnumerable<DadosDeLeituraDoComunicadoPorModalidadeETurmaDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IRepositorioComunicado repositorioComunicado;

        public ObterDadosDeLeituraDeComunicadosPorModalidadePorTurmaQueryHandler(IHttpClientFactory httpClientFactory, IRepositorioComunicado repositorioComunicado)
        {
            this.httpClientFactory = httpClientFactory;
            this.repositorioComunicado = repositorioComunicado;
        }

        public async Task<IEnumerable<DadosDeLeituraDoComunicadoPorModalidadeETurmaDto>> Handle(ObterDadosDeLeituraDeComunicadosPorModalidadePorTurmaQuery request, CancellationToken cancellationToken)
        {
            if (!await repositorioComunicado.Exists(request.ComunicadoId))
                throw new NegocioException("O comunicado informado não existe. Por favor tente novamente.", HttpStatusCode.BadRequest);

            var httpClient = httpClientFactory.CreateClient("servicoAcompanhamentoEscolar");
            var url = new StringBuilder("/api/v1/dashboard/leitura/turma");

            url.Append(@"?codigoDre=" + request.CodigoDre);
            url.Append(@"&codigoUe=" + request.CodigoUe);
            url.Append(@"&notificacaoId=" + request.ComunicadoId);
            url.Append(@"&modoVisualizacao=" + request.ModoVisualizacao);
            if (request.CodigosTurmas != null)
            {
                foreach (var item in request.CodigosTurmas)
                {                    
                    url.Append(@"&codigosTurmas=" + item);
                }
            }
            if (request.Modalidades != null)
            {
                foreach (var item in request.Modalidades)
                {
                    url.Append(@"&modalidades=" + item);
                }
            }

            var resposta = await httpClient.GetAsync($"{url}", cancellationToken);
            if (!resposta.IsSuccessStatusCode || resposta.StatusCode == HttpStatusCode.NoContent)
                throw new NegocioException("Não foram encontrados dados de leitura de comunicados.", HttpStatusCode.InternalServerError);

            var json = await resposta.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<DadosDeLeituraDoComunicadoPorModalidadeETurmaDto>>(json);
        }
    }
}