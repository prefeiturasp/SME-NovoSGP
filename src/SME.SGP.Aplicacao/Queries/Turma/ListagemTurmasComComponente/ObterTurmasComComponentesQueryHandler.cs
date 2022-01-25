using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasComComponentesQueryHandler : IRequestHandler<ObterTurmasComComponentesQuery, PaginacaoResultadoDto<RetornoConsultaListagemTurmaComponenteDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterTurmasComComponentesQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<PaginacaoResultadoDto<RetornoConsultaListagemTurmaComponenteDto>> Handle(ObterTurmasComComponentesQuery request, CancellationToken cancellationToken)
        {
            var turmas = new PaginacaoResultadoDto<RetornoConsultaListagemTurmaComponenteDto>();

            var turmaCodigo = request.TurmaCodigo == null ? 0 : long.Parse(request.TurmaCodigo);

            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var resposta = await httpClient.GetAsync($"turmas/ues/{request.UeCodigo}/modalidades/{(int)request.Modalidade}/anos/{request.AnoLetivo}/componentes?codigoTurma={turmaCodigo}&ehProfessor={request.EhProfessor}&codigoRf={request.CodigoRf}&qtdeRegistros={request.QtdeRegistros}&qtdeRegistrosIgnorados={request.QtdeRegistrosIgnorados}&componentesCurricularesDoProfessorCJ={request.ComponentesCurricularesDoProfessorCJ}&consideraHistorico={request.ConsideraHistorico}");

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                turmas = JsonConvert.DeserializeObject<PaginacaoResultadoDto<RetornoConsultaListagemTurmaComponenteDto>>(json);
            }
            return turmas;
        }
    }
}
