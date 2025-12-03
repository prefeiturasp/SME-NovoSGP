using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPrioridadeEncaminhamentoNAAPAUseCase : IObterPrioridadeEncaminhamentoNAAPAUseCase
    {
        private readonly IMediator mediator;

        public ObterPrioridadeEncaminhamentoNAAPAUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<PrioridadeAtendimentoNAAPADto>> Executar()
        {
            return await mediator.Send(ObterPrioridadeEncaminhamentoNAAPAQuery.Instance);
        }
    }
}
