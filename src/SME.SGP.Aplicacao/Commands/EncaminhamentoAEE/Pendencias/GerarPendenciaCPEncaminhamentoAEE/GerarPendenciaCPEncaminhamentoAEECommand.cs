using MediatR;

namespace SME.SGP.Aplicacao
{
    public class GerarPendenciaCPEncaminhamentoAEECommand : IRequest<bool>
    {
        public long EncaminhamentoAEEId { get; set; }

        public GerarPendenciaCPEncaminhamentoAEECommand(long encaminhamentoAEEId)
        {
            EncaminhamentoAEEId = encaminhamentoAEEId;
        }
    }

}
