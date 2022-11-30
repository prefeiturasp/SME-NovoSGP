using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorTurmaCodigoQueryHandler : IRequestHandler<ObterComponentesCurricularesPorTurmaCodigoQuery, IEnumerable<DisciplinaResposta>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public ObterComponentesCurricularesPorTurmaCodigoQueryHandler(IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular,IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }
        
        public async Task<IEnumerable<DisciplinaResposta>> Handle(ObterComponentesCurricularesPorTurmaCodigoQuery request, CancellationToken cancellationToken)
        {
            var url = $"funcionarios/turmas/{request.TurmaCodigo}/disciplinas";
            
            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            
            var resposta = await httpClient.GetAsync(url);

            if (resposta.IsSuccessStatusCode && resposta.StatusCode != HttpStatusCode.NoContent)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<DisciplinaResposta>>(json);
            }
            return null;
        }
    }
}
