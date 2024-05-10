using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterWorkflowAprovacaoNotaFechamentoComAprovacaoIdQueryHandler : IRequestHandler<ObterWorkflowAprovacaoNotaFechamentoComAprovacaoIdQuery, IEnumerable<WfAprovacaoNotaFechamentoTurmaDto>>
    {
        private readonly IRepositorioWfAprovacaoNotaFechamento repositorioWfAprovacaoNotaFechamento;
        public ObterWorkflowAprovacaoNotaFechamentoComAprovacaoIdQueryHandler(IRepositorioWfAprovacaoNotaFechamento repositorioWfAprovacaoNotaFechamento)
        {
            this.repositorioWfAprovacaoNotaFechamento = repositorioWfAprovacaoNotaFechamento ?? throw new ArgumentNullException(nameof(repositorioWfAprovacaoNotaFechamento));
        }
        public async Task<IEnumerable<WfAprovacaoNotaFechamentoTurmaDto>> Handle(ObterWorkflowAprovacaoNotaFechamentoComAprovacaoIdQuery request, CancellationToken cancellationToken)
          => await repositorioWfAprovacaoNotaFechamento.ObterWfAprovacaoNotaFechamentoComWfAprovacaoId(request.WorkflowId);
    }
}
