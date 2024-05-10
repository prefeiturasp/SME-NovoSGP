using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarWorkflowAprovacaoCommandHandler : IRequestHandler<SalvarWorkflowAprovacaoCommand, long>
    {
        private readonly IRepositorioWorkflowAprovacao repositorioWorkflowAprovacao;

        public SalvarWorkflowAprovacaoCommandHandler(IRepositorioWorkflowAprovacao repositorioWorkflowAprovacao)
        {
            this.repositorioWorkflowAprovacao = repositorioWorkflowAprovacao ?? throw new ArgumentNullException(nameof(repositorioWorkflowAprovacao));
        }

        public async Task<long> Handle(SalvarWorkflowAprovacaoCommand request, CancellationToken cancellationToken)
        {
            await repositorioWorkflowAprovacao.SalvarAsync(request.WorkflowAprovacao);

            return request.WorkflowAprovacao.Id;
        }
    }
}
