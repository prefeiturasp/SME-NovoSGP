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
    public class ObterWfsAprovacaoNotaConselhoPorWorkflowQueryHandler : IRequestHandler<ObterWfsAprovacaoNotaConselhoPorWorkflowQuery, IEnumerable<WFAprovacaoNotaConselho>>
    {
        private readonly IRepositorioWFAprovacaoNotaConselho repositorio;

        public ObterWfsAprovacaoNotaConselhoPorWorkflowQueryHandler(IRepositorioWFAprovacaoNotaConselho repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<WFAprovacaoNotaConselho>> Handle(ObterWfsAprovacaoNotaConselhoPorWorkflowQuery request, CancellationToken cancellationToken)
            => await repositorio.ObterWfsAprovacaoPorWorkflow(request.WorkflowId);
    }
}
