using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao 
{
    class ObterNotaFechamentoEmAprovacaoPorWorkflowIdQueryHandler : IRequestHandler<ObterNotaFechamentoEmAprovacaoPorWorkflowIdQuery, IEnumerable<WfAprovacaoNotaFechamentoTurmaDto>>
    {
        private readonly IRepositorioFechamentoNotaConsulta repositorioFechamentoNota;

        public ObterNotaFechamentoEmAprovacaoPorWorkflowIdQueryHandler(IRepositorioFechamentoNotaConsulta repositorioFechamentoNota)
        {
            this.repositorioFechamentoNota = repositorioFechamentoNota ?? throw new ArgumentNullException(nameof(repositorioFechamentoNota));
        }

        public async Task<IEnumerable<WfAprovacaoNotaFechamentoTurmaDto>> Handle(ObterNotaFechamentoEmAprovacaoPorWorkflowIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioFechamentoNota.ObterNotasEmAprovacaoWf(request.WorkflowId);
        }
    }
}
