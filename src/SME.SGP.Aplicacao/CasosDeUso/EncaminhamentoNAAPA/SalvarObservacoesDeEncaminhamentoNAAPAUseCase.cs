using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarObservacoesDeEncaminhamentoNAAPAUseCase : ISalvarObservacoesDeEncaminhamentoNAAPAUseCase
    {
        public readonly IMediator mediator;

        public SalvarObservacoesDeEncaminhamentoNAAPAUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(EncaminhamentoNAAPAObservacaoSalvarDto filtro)
        {
            return await mediator.Send(new SalvarObservacaoNAAPACommand(filtro));
        }
    }
}