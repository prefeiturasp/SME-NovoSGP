using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.AnotacaoFrequenciaAluno;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAnotacaoFrequenciaAlunoPorPeriodoUseCase : AbstractUseCase, IObterAnotacaoFrequenciaAlunoPorPeriodoUseCase
    {
        public ObterAnotacaoFrequenciaAlunoPorPeriodoUseCase(IMediator mediator) : base(mediator) { }

        public async Task<IEnumerable<AnotacaoAlunoAulaPorPeriodoDto>> Executar(FiltroAnotacaoFrequenciaAlunoPorPeriodoDto param)
         => await mediator.Send(new ObterAnotacaoFrequenciaAlunoPorPeriodoQuery(param.CodigoAluno, param.DataInicio, param.DataFim));

    }
}
