using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes
{
    public class ObterComponentesCurricularesRegenciaPorAnoETurnoQueryFake : IRequestHandler<ObterComponentesCurricularesRegenciaPorAnoETurnoQuery, IEnumerable<DisciplinaDto>>
    {
        public async Task<IEnumerable<DisciplinaDto>> Handle(ObterComponentesCurricularesRegenciaPorAnoETurnoQuery request, CancellationToken cancellationToken)
        {
            return new List<DisciplinaDto>()
            {
                new DisciplinaDto()
                {
                    CodigoComponenteCurricular = 2
                }
            };
        }
    }
}
