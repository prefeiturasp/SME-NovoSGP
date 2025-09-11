using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciaDiarioPorAulaIdCommand : IRequest<bool>
    {
        public ExcluirPendenciaDiarioPorAulaIdCommand(long aulaId)
        {
            AulaId = aulaId;
        }

        public long AulaId { get; set; }
    }
}
