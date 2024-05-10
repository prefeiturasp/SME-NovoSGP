using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Aula.ServicosFake
{
    public class ObterComponenteCurricularPorIdQueryHandlerFake : IRequestHandler<ObterComponenteCurricularPorIdQuery, DisciplinaDto>
    {
        public async Task<DisciplinaDto> Handle(ObterComponenteCurricularPorIdQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new DisciplinaDto()
            {
                CodigoComponenteCurricular = 1                
            });
        }
    }
}
