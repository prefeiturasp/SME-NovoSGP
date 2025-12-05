using System;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class SalvarObservacoesDeAtendimentoNAAPAUseCase : ISalvarObservacoesDeAtendimentoNAAPAUseCase
    {
        public readonly IMediator mediator;

        public SalvarObservacoesDeAtendimentoNAAPAUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(AtendimentoNAAPAObservacaoSalvarDto filtro)
        {
            return await mediator.Send(new SalvarObservacaoNAAPACommand(filtro));
        }
    }
}