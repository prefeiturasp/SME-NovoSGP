using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao 
{
    class ObterNotaFechamentoEmAprovacaoPorWorkflowIdQueryHandler : IRequestHandler<ObterNotaFechamentoEmAprovacaoPorWorkflowIdQuery, IEnumerable<WfAprovacaoNotaFechamento>>
    {
        private readonly IRepositorioFechamentoNotaConsulta repositorioFechamentoNota;

        public ObterNotaFechamentoEmAprovacaoPorWorkflowIdQueryHandler(IRepositorioFechamentoNotaConsulta repositorioFechamentoNota)
        {
            this.repositorioFechamentoNota = repositorioFechamentoNota ?? throw new ArgumentNullException(nameof(repositorioFechamentoNota));
        }

        public async Task<IEnumerable<WfAprovacaoNotaFechamento>> Handle(ObterNotaFechamentoEmAprovacaoPorWorkflowIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioFechamentoNota.ObterNotasEmAprovacaoWf(request.WorkflowId);
        }
    }
}
