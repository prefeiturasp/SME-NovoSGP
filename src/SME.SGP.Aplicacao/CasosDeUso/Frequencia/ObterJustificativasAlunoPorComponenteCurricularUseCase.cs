using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterJustificativasAlunoPorComponenteCurricularUseCase : AbstractUseCase, IObterJustificativasAlunoPorComponenteCurricularUseCase
    {
        public ObterJustificativasAlunoPorComponenteCurricularUseCase(IMediator mediator) : base(mediator)
        {

        }

        public Task<IEnumerable<JustificativaAlunoDto>> Executar(FiltroJustificativasAlunoPorComponenteCurricular param)
        {
            throw new System.NotImplementedException();
        }
    }
}
