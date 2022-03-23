using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    class ObterCriadorWorkflowQueryHandler : IRequestHandler<ObterCriadorWorkflowQuery, string>
    {
        private readonly IRepositorioWorkflowAprovacao repositorioWorkflowAprovacao;

        public ObterCriadorWorkflowQueryHandler(IRepositorioWorkflowAprovacao repositorioWorkflowAprovacao)
        {
            this.repositorioWorkflowAprovacao = repositorioWorkflowAprovacao ?? throw new ArgumentNullException(nameof(repositorioWorkflowAprovacao));
        }

        public async Task<string> Handle(ObterCriadorWorkflowQuery request, CancellationToken cancellationToken)
            => await repositorioWorkflowAprovacao.ObterCriador(request.WorkflowId);
    }
}
