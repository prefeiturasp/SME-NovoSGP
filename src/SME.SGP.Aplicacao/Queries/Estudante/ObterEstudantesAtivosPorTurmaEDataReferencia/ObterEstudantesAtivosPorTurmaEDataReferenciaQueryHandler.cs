using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterEstudantesAtivosPorTurmaEDataReferenciaQueryHandler : IRequestHandler<ObterEstudantesAtivosPorTurmaEDataReferenciaQuery, IEnumerable<EstudanteDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterEstudantesAtivosPorTurmaEDataReferenciaQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<EstudanteDto>> Handle(ObterEstudantesAtivosPorTurmaEDataReferenciaQuery request, CancellationToken cancellationToken)
        {
            var alunos = new List<EstudanteDto>();

            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var resposta = await httpClient.GetAsync($"alunos/turmas/{request.TurmaCodigo}/ativos");
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                alunos = JsonConvert.DeserializeObject<List<EstudanteDto>>(json);
            }
            return alunos;
        }
    }
}
