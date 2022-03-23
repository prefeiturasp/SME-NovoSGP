using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterParecerConclusivoEmAprovacaoPorWorkflowQueryHandler : IRequestHandler<ObterParecerConclusivoEmAprovacaoPorWorkflowQuery, WFAprovacaoParecerConclusivo>
    {
        private readonly IRepositorioWFAprovacaoParecerConclusivo repositorioWFAprovacaoParecerConclusivo;

        public ObterParecerConclusivoEmAprovacaoPorWorkflowQueryHandler(IRepositorioWFAprovacaoParecerConclusivo repositorioWFAprovacaoParecerConclusivo)
        {
            this.repositorioWFAprovacaoParecerConclusivo = repositorioWFAprovacaoParecerConclusivo ?? throw new ArgumentNullException(nameof(repositorioWFAprovacaoParecerConclusivo));
        }

        public async Task<WFAprovacaoParecerConclusivo> Handle(ObterParecerConclusivoEmAprovacaoPorWorkflowQuery request, CancellationToken cancellationToken)
        {
            return await repositorioWFAprovacaoParecerConclusivo.ObterPorWorkflowId(request.WorkflowId);
        }
    }
}
