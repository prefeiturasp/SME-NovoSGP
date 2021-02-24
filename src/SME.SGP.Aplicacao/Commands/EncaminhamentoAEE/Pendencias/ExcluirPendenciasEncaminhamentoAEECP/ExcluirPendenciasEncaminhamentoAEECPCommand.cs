using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciasEncaminhamentoAEECPCommand : IRequest<bool>
    {
        public ExcluirPendenciasEncaminhamentoAEECPCommand(long pendenciaId, long turmaId)
        {
            PendenciaId = pendenciaId;
            TurmaId = turmaId;
        }

        public long TurmaId { get; set; }
        public long PendenciaId { get; set; }
    }
}
