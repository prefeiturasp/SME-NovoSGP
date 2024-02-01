using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
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
