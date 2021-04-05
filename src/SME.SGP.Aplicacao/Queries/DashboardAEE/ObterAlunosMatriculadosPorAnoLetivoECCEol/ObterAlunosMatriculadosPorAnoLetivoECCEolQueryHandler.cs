using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosMatriculadosPorAnoLetivoECCEolQueryHandler : IRequestHandler<ObterAlunosMatriculadosPorAnoLetivoECCEolQuery, IEnumerable<AlunosMatriculadosEolDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterAlunosMatriculadosPorAnoLetivoECCEolQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<IEnumerable<AlunosMatriculadosEolDto>> Handle(ObterAlunosMatriculadosPorAnoLetivoECCEolQuery request, CancellationToken cancellationToken)
        {
            var alunos = new List<AlunosMatriculadosEolDto>();
            var componentesCurriculares = String.Join("&componentesCurriculares=", request.ComponentesCurriculares);

            var url = $"alunos/ano-letivo/{request.Ano}/matriculados?componentesCurriculares={componentesCurriculares}";
            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            if (request.DreId > 0)
                url += $"&dreId={request.DreId}";

            if (request.UeId > 0)
                url += $"&ueId={request.UeId}";

            var resposta = await httpClient.GetAsync(url);
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                alunos = JsonConvert.DeserializeObject<List<AlunosMatriculadosEolDto>>(json);
            }
            return alunos;
        }
           
    }
}
