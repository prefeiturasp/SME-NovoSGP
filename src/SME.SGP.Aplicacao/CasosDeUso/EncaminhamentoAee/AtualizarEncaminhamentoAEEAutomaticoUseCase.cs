using MediatR;
using SME.SGP.Aplicacao.Commands;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtualizarEncaminhamentoAEEAutomaticoUseCase : AbstractUseCase, IAtualizarEncaminhamentoAEEAutomaticoUseCase
    {
        public AtualizarEncaminhamentoAEEAutomaticoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<FiltroAtualizarEncaminhamentoAEEEncerramentoAutomaticoDto>();
            await mediator.Send(new AtualizarEncaminhamentoAEEEncerrarAutomaticoCommand(filtro.EncaminhamentoId));

            return true;
        }
    }
}
