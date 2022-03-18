using MediatR;

namespace SME.SGP.Aplicacao.Commands
{
    public class AtualizarEncaminhamentoAEEEncerrarAutomaticoCommand : IRequest
    {
        public AtualizarEncaminhamentoAEEEncerrarAutomaticoCommand(long encaminhamentoId)
        {
            EncaminhamentoId = encaminhamentoId;
        }

        public long EncaminhamentoId { get; }
    }
}
