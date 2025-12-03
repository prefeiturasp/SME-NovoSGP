using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ReabrirAtendimentoNAAPAUseCase : IReabrirAtendimentoNAAPAUseCase
    {
        private readonly IMediator mediator;

        public ReabrirAtendimentoNAAPAUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<SituacaoDto> Executar(long encaminhamentoId)
        {
            return await mediator.Send(new ReabrirEncaminhamentoNAAPACommand(encaminhamentoId));
        }
    }
}
