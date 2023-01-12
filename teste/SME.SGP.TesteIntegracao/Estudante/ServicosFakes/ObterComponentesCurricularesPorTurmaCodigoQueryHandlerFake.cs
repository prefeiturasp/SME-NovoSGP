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
    public class ObterComponentesCurricularesPorTurmaCodigoQueryHandler_TurmasProgramaEstudanteFake : IRequestHandler<ObterComponentesCurricularesPorTurmaCodigoQuery, IEnumerable<DisciplinaResposta>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        
        public async Task<IEnumerable<DisciplinaResposta>> Handle(ObterComponentesCurricularesPorTurmaCodigoQuery request, CancellationToken cancellationToken)
        {
            return new List<DisciplinaResposta>()
            {
              new DisciplinaResposta()
              {
                CodigoComponenteCurricular = 1,
                Nome = "RECUPERAÇÃO DE APRENDIZAGENS"
              }
            };
        }
    }
}
