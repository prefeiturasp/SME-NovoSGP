using System;
using System.Threading.Tasks;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirObservacoesDeAtendimentoNAAPAUseCase : IExcluirObservacoesDeAtendimentoNAAPAUseCase
    {
        private readonly IMediator mediator;

        public ExcluirObservacoesDeAtendimentoNAAPAUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(long observacaoId)
        {
            return await mediator.Send(new ExcluirObservacaoNAAPACommand(observacaoId));
        }
    }
}