using MediatR;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorTurmaCodigoQueryHandler_TurmasProgramaEstudanteFake : IRequestHandler<ObterDisciplinasPorCodigoTurmaQuery, IEnumerable<DisciplinaResposta>>
    {
        public async Task<IEnumerable<DisciplinaResposta>> Handle(ObterDisciplinasPorCodigoTurmaQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new List<DisciplinaResposta>()
            {
              new DisciplinaResposta()
              {
                CodigoComponenteCurricular = 1,
                Nome = "RECUPERAÇÃO DE APRENDIZAGENS"
              }
            });
        }
    }
}
