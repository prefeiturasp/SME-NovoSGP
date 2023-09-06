using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirWorkflowCommandHandler : IRequestHandler<ExcluirWorkflowCommand, bool>
    {
        private readonly IRepositorioWorkflowAprovacao repositorioWorkflowAprovacao;
        private readonly IRepositorioWorkflowAprovacaoNivel repositorioWorkflowAprovacaoNivel;
        private readonly IRepositorioWorkflowAprovacaoNivelNotificacao repositorioWorkflowAprovacaoNivelNotificacao;
        private readonly IMediator mediator;

        public ExcluirWorkflowCommandHandler(IRepositorioWorkflowAprovacao repositorioWorkflowAprovacao,
                                             IRepositorioWorkflowAprovacaoNivel repositorioWorkflowAprovacaoNivel,
                                             IRepositorioWorkflowAprovacaoNivelNotificacao repositorioWorkflowAprovacaoNivelNotificacao,
                                             IMediator mediator)
        {
            this.repositorioWorkflowAprovacao = repositorioWorkflowAprovacao ?? throw new ArgumentNullException(nameof(repositorioWorkflowAprovacao));
            this.repositorioWorkflowAprovacaoNivel = repositorioWorkflowAprovacaoNivel ?? throw new ArgumentNullException(nameof(repositorioWorkflowAprovacaoNivel));
            this.repositorioWorkflowAprovacaoNivelNotificacao = repositorioWorkflowAprovacaoNivelNotificacao ?? throw new ArgumentNullException(nameof(repositorioWorkflowAprovacaoNivelNotificacao));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExcluirWorkflowCommand request, CancellationToken cancellationToken)
        {
            var workflow = await repositorioWorkflowAprovacao.ObterEntidadeCompleta(request.WorkflowId);

            if (workflow == null)
                throw new NegocioException("Não foi possível localizar o fluxo de aprovação.");

            if (workflow.Niveis.Any(n => n.Status == WorkflowAprovacaoNivelStatus.Reprovado))
                return false;

            foreach (WorkflowAprovacaoNivel wfNivel in workflow.Niveis)
            {
                wfNivel.Status = WorkflowAprovacaoNivelStatus.Excluido;
                repositorioWorkflowAprovacaoNivel.Salvar(wfNivel);

                foreach (Notificacao notificacao in wfNivel.Notificacoes)
                {
                    await repositorioWorkflowAprovacaoNivelNotificacao.ExcluirPorWorkflowNivelNotificacaoId(wfNivel.Id, notificacao.Id);
                    await mediator.Send(new ExcluirNotificacaoCommand(notificacao));
                }
            }

            workflow.Excluido = true;
            await repositorioWorkflowAprovacao.SalvarAsync(workflow);

            return true;
        }
    }
}
