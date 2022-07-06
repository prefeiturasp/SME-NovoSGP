using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterWorkflowAprovacaoNotaFechamentoSemAprovacaoIdQueryHandler : IRequestHandler<ObterWorkflowAprovacaoNotaFechamentoSemAprovacaoIdQuery, IEnumerable<WfAprovacaoNotaFechamentoTurmaDto>>
    {
        private readonly IRepositorioWfAprovacaoNotaFechamento repositorioWfAprovacaoNotaFechamento;
        public ObterWorkflowAprovacaoNotaFechamentoSemAprovacaoIdQueryHandler(IRepositorioWfAprovacaoNotaFechamento repositorioWfAprovacaoNotaFechamento)
        {
            this.repositorioWfAprovacaoNotaFechamento = repositorioWfAprovacaoNotaFechamento ?? throw new ArgumentNullException(nameof(repositorioWfAprovacaoNotaFechamento));
        }
        public async Task<IEnumerable<WfAprovacaoNotaFechamentoTurmaDto>> Handle(ObterWorkflowAprovacaoNotaFechamentoSemAprovacaoIdQuery request, CancellationToken cancellationToken)
          => await repositorioWfAprovacaoNotaFechamento.ObterWfAprovacaoNotaFechamentoSemWfAprovacaoId();
    }
}
