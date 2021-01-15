using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands
{
    public class AlteraWorkflowAprovacaoNotificacaoCargoCommandHandler : IRequestHandler<AlteraWorkflowAprovacaoNotificacaoCargoCommand, bool>
    {
        private readonly IRepositorioWorkflowAprovacao repositorioWorkflowAprovacao;
        private readonly IMediator mediator;
        private readonly IRepositorioWorkflowAprovacaoNivelNotificacao repositorioWorkflowAprovacaoNivelNotificacao;
        private readonly IRepositorioNotificacao repositorioNotificacao;

        public AlteraWorkflowAprovacaoNotificacaoCargoCommandHandler(IRepositorioWorkflowAprovacao repositorioWorkflowAprovacao,
            IMediator mediator, IRepositorioWorkflowAprovacaoNivelNotificacao repositorioWorkflowAprovacaoNivelNotificacao, IRepositorioNotificacao repositorioNotificacao)
        {
            this.repositorioWorkflowAprovacao = repositorioWorkflowAprovacao ?? throw new ArgumentNullException(nameof(repositorioWorkflowAprovacao));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioWorkflowAprovacaoNivelNotificacao = repositorioWorkflowAprovacaoNivelNotificacao ?? throw new ArgumentNullException(nameof(repositorioWorkflowAprovacaoNivelNotificacao));
            this.repositorioNotificacao = repositorioNotificacao ?? throw new ArgumentNullException(nameof(repositorioNotificacao));
        }
        public async Task<bool> Handle(AlteraWorkflowAprovacaoNotificacaoCargoCommand request, CancellationToken cancellationToken)
        {
            var wfAprovacao = repositorioWorkflowAprovacao.ObterEntidadeCompleta(request.WorkflowId);
            if (wfAprovacao == null)
                throw new NegocioException("Não foi possível obter o workflow de aprovação.");

            var nivelParaModificar = wfAprovacao.ObterNivelPorNotificacaoId(request.NotificacaoId);
            if (nivelParaModificar == null)
                throw new NegocioException("Não foi possível obter o nível do workflow de aprovação.");


            //Verifico se tem notificação para excluir, quando o funcionário não está mais no cargo
            var listaIds = new List<long>();

            foreach (var notificacao in nivelParaModificar.Notificacoes.Where(a => !a.Excluida))
            {
                if (!request.FuncionariosCargos.Any(a => a.FuncionarioRF == notificacao.Usuario.CodigoRf))
                {
                    listaIds.Add(notificacao.Id);
                }
            }

            if (listaIds.Any())
                await repositorioNotificacao.ExcluirLogicamentePorIdsAsync(listaIds.ToArray());


            //Verifico se os funcionários no nível tem notificação
            foreach (var funcionario in request.FuncionariosCargos)
            {
                if (!nivelParaModificar.Notificacoes.Where(a => !a.Excluida).Any(a => a.Usuario.CodigoRf == funcionario.FuncionarioRF))
                {
                    await TrataNovoFuncionarioNivel(wfAprovacao, nivelParaModificar, funcionario.FuncionarioRF);
                }
            }


            return true;

        }

        private async Task TrataNovoFuncionarioNivel(WorkflowAprovacao wfAprovacao, WorkflowAprovacaoNivel nivelDoCargo, string funcionarioRF)
        {
            var notificacaoBase = wfAprovacao.Niveis.Where(a => a.Notificacoes.Any()).SelectMany(a => a.Notificacoes).FirstOrDefault();

            var notificarUsuarioCommand = new NotificarUsuarioCommand(
          wfAprovacao.NotifacaoTitulo,
          wfAprovacao.NotifacaoMensagem,
          funcionarioRF,
          (NotificacaoCategoria)wfAprovacao.NotificacaoCategoria,
          (NotificacaoTipo)wfAprovacao.NotificacaoTipo,
          wfAprovacao.DreId,
          wfAprovacao.UeId,
          wfAprovacao.TurmaId,
          wfAprovacao.Ano,
          notificacaoBase.Codigo,
          notificacaoBase.CriadoEm);

            var notificacaoId = await mediator.Send(notificarUsuarioCommand);

            repositorioWorkflowAprovacaoNivelNotificacao.Salvar(new WorkflowAprovacaoNivelNotificacao()
            {
                NotificacaoId = notificacaoId,
                WorkflowAprovacaoNivelId = nivelDoCargo.Id
            });
        }
    }
}
