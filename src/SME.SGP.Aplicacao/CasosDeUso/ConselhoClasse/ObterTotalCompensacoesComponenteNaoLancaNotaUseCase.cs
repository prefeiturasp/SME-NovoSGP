using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalCompensacoesComponenteNaoLancaNotaUseCase : IObterTotalCompensacoesComponenteNaoLancaNotaUseCase
    {
        private readonly IMediator mediator;
        public ObterTotalCompensacoesComponenteNaoLancaNotaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<TotalCompensacoesComponenteNaoLancaNotaDto>> Executar(string codigoTurma, int bimestre)
        {
            return await mediator.Send(new ObterTotalCompensacoesComponenteNaoLancaNotaQuery(codigoTurma,bimestre));
        }

    }
}
