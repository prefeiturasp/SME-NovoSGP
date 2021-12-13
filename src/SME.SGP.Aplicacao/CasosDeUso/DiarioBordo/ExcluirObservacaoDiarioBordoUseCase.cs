using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirObservacaoDiarioBordoUseCase : IExcluirObservacaoDiarioBordoUseCase
    {
        private readonly IMediator mediator;

        public ExcluirObservacaoDiarioBordoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(long diarioBordoObservacaoId)
        {
            return await mediator.Send(new ExcluirObservacaoDiarioBordoCommand(diarioBordoObservacaoId));
        }
    }
}
