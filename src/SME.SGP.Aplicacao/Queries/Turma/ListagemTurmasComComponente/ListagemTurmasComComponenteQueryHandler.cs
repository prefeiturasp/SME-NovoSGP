using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ListagemTurmasComComponenteQueryHandler : IRequestHandler<ListagemTurmasComComponenteQuery, PaginacaoResultadoDto<RetornoConsultaListagemTurmaComponenteDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ListagemTurmasComComponenteQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<PaginacaoResultadoDto<RetornoConsultaListagemTurmaComponenteDto>> Handle(ListagemTurmasComComponenteQuery request, CancellationToken cancellationToken)
        {
            var turmas = new PaginacaoResultadoDto<RetornoConsultaListagemTurmaComponenteDto>();

            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var resposta = await httpClient.GetAsync($"turmas/{request.UeCodigo}/{request.Modalidade}/{request.Bimestre}/{request.TurmaCodigo}/{request.AnoLetivo}/{request.QtdeRegistros}/{request.QtdeRegistrosIgnorados}/listagem-turmas");
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                turmas = JsonConvert.DeserializeObject<PaginacaoResultadoDto<RetornoConsultaListagemTurmaComponenteDto>>(json);
            }
            return turmas;
        }
    }
}
