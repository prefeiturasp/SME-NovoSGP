using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao.Commands
{
    public class AtualizarEncaminhamentoAEEEncerrarAutomaticoCommand : IRequest<EncaminhamentoAEE>
    {
        public AtualizarEncaminhamentoAEEEncerrarAutomaticoCommand(long encaminhamentoId)
        {
            EncaminhamentoId = encaminhamentoId;
        }

        public long EncaminhamentoId { get; }
    }
}
