using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciasEncaminhamentoAEECEFAICommand : IRequest<bool>
    {
        public ExcluirPendenciasEncaminhamentoAEECEFAICommand(long turmaId, long encaminhamentoAEEId)
        {
            TurmaId = turmaId;
            EncaminhamentoAEEId = encaminhamentoAEEId;
        }

        public long EncaminhamentoAEEId { get; set; }
        public long TurmaId { get; set; }
    }
}
