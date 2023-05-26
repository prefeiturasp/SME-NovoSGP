using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterIdsWorkflowPorWfAprovacaoIdQueryHandler : IRequestHandler<ObterIdsWorkflowPorWfAprovacaoIdQuery, IEnumerable<long>>
    {
        private readonly IRepositorioWorkflowAprovacao repositorioWorkflowAprovacao;

        public ObterIdsWorkflowPorWfAprovacaoIdQueryHandler(IRepositorioWorkflowAprovacao repositorioWorkflowAprovacao)
        {
            this.repositorioWorkflowAprovacao = repositorioWorkflowAprovacao ?? throw new ArgumentNullException(nameof(repositorioWorkflowAprovacao));
        }

        public async Task<IEnumerable<long>> Handle(ObterIdsWorkflowPorWfAprovacaoIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioWorkflowAprovacao.ObterIdsWorkflowPorWfAprovacaoId(request.WorkflowId, request.TabelaVinculada);
        }
    }
}
