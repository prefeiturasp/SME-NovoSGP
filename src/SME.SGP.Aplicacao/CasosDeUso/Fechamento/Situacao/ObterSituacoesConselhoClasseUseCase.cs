using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterSituacoesConselhoClasseUseCase : IObterSituacoesConselhoClasseUseCase
    {

        private readonly IMediator mediator;

        public ObterSituacoesConselhoClasseUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<List<SituacaoDto>> Executar()
        {
            return await mediator.Send(new ObterSituacoesConselhoClasseQuery());
        }

    }
}
