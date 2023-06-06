using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterIdsWorkflowPorWfAprovacaoIdQuery : IRequest<IEnumerable<long>>
    {
        public ObterIdsWorkflowPorWfAprovacaoIdQuery(long workflowId, string tabelaVinculada)
        {
            WorkflowId = workflowId;
            TabelaVinculada = tabelaVinculada;
        }
        public long WorkflowId { get; set; }
        public string TabelaVinculada { get; }
    }
}
