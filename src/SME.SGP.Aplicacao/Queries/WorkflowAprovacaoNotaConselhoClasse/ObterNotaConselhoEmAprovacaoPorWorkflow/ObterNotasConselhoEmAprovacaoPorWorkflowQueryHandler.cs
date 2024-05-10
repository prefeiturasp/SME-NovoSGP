using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasConselhoEmAprovacaoPorWorkflowQueryHandler : IRequestHandler<ObterNotasConselhoEmAprovacaoPorWorkflowQuery, IEnumerable<WFAprovacaoNotaConselho>>
    {
        private readonly IRepositorioWFAprovacaoNotaConselho repositorio;

        public ObterNotasConselhoEmAprovacaoPorWorkflowQueryHandler(IRepositorioWFAprovacaoNotaConselho repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<WFAprovacaoNotaConselho>> Handle(ObterNotasConselhoEmAprovacaoPorWorkflowQuery request, CancellationToken cancellationToken)
            => await repositorio.ObterNotasEmAprovacaoPorWorkflow(request.WorkflowId);
    }
}
