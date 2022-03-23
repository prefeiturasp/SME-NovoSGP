using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNotaConselhoEmAprovacaoPorWorkflowQueryHandler : IRequestHandler<ObterNotaConselhoEmAprovacaoPorWorkflowQuery, WFAprovacaoNotaConselho>
    {
        private readonly IRepositorioWFAprovacaoNotaConselho repositorio;

        public ObterNotaConselhoEmAprovacaoPorWorkflowQueryHandler(IRepositorioWFAprovacaoNotaConselho repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<WFAprovacaoNotaConselho> Handle(ObterNotaConselhoEmAprovacaoPorWorkflowQuery request, CancellationToken cancellationToken)
            => await repositorio.ObterNotaEmAprovacaoPorWorkflow(request.WorkflowId);
    }
}
