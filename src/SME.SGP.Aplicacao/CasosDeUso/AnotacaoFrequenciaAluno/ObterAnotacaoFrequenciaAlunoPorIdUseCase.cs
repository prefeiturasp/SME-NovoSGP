using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAnotacaoFrequenciaAlunoPorIdUseCase : AbstractUseCase, IObterAnotacaoFrequenciaAlunoPorIdUseCase
    {
        public ObterAnotacaoFrequenciaAlunoPorIdUseCase(IMediator mediator) : base(mediator) {}

        public Task<AnotacaoFrequenciaAlunoCompletoDto> Executar(long id)
        {
            throw new System.NotImplementedException();
        }
    }
}
