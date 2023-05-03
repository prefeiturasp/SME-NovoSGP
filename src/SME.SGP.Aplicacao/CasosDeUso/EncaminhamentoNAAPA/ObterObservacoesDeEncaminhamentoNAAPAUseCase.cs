using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterObservacoesDeEncaminhamentoNAAPAUseCase : IObterObservacoesDeEncaminhamentoNAAPAUseCase
    {
        private readonly IMediator mediator;

        public ObterObservacoesDeEncaminhamentoNAAPAUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<PaginacaoResultadoDto<EncaminhamentoNAAPAObservacoesDto>> Executar(long encaminhamentoNAAPAId)
        {
            var usuarioLogadoId = await mediator.Send(new ObterUsuarioLogadoIdQuery());
            return await mediator.Send(new ObterObservacaoEncaminhamentosNAAPAQuery(encaminhamentoNAAPAId, usuarioLogadoId));
        }
    }
}
