using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoWorkflowAprovacao : IServicoWorkflowAprovacao
    {
        private readonly IRepositorioNotificacao repositorioNotificacao;
        private readonly IRepositorioWorkflowAprovacao repositorioWorkflowAprovacao;
        private readonly IUnitOfWork unitOfWork;

        public ServicoWorkflowAprovacao(IRepositorioWorkflowAprovacao repositorioWorkflowAprovacao, IUnitOfWork unitOfWork,
            IRepositorioNotificacao repositorioNotificacao)
        {
            this.repositorioWorkflowAprovacao = repositorioWorkflowAprovacao ?? throw new System.ArgumentNullException(nameof(repositorioWorkflowAprovacao));
            this.unitOfWork = unitOfWork ?? throw new System.ArgumentNullException(nameof(unitOfWork));
            this.repositorioNotificacao = repositorioNotificacao ?? throw new System.ArgumentNullException(nameof(repositorioNotificacao));
        }

        public void ConfiguracaoInicial(long id)
        {
            var workflow = repositorioWorkflowAprovacao.ObterComNiveisPorId(id);

            if (workflow == null)
                throw new NegocioException($"Não foi possível localizar este workflow de id {id}");

            var niveisIniciais = workflow.ObtemNiveis(workflow.ObtemPrimeiroNivel());

            //var primeiroNivel = workflowAprovaNiveis.OrderBy(a => a.Nivel)
            //    .FirstOrDefault()
            //    .Nivel;

            //var primeirosNiveis = workflowAprovaNiveis
            //    .Where(a => a.Nivel == primeiroNivel)
            //    .ToList();
        }

        private void EnviaNotificacaoParaNiveis(List<WorkflowAprovacao> aprovaNiveis)
        {
            unitOfWork.IniciarTransacao();
            foreach (var aprovaNivel in aprovaNiveis)
            {
                EnviaNotificacaoParaNivel(aprovaNivel);
            }
            unitOfWork.PersistirTransacao();
        }

        private void EnviaNotificacaoParaNivel(WorkflowAprovacao aprovaNivel)
        {
            var notificacao = new Notificacao()
            {
                Ano = aprovaNivel.Ano,
                Categoria = NotificacaoCategoria.Workflow_Aprovacao,
                DreId = aprovaNivel.DreId,
                UeId = aprovaNivel.UeId,
                Mensagem = aprovaNivel.NotifacaoMensagem,
                Tipo = aprovaNivel.NotificacaoTipo,
                Titulo = aprovaNivel.NotifacaoTitulo,
                TurmaId = aprovaNivel.TurmaId
            };
            repositorioNotificacao.Salvar(notificacao);

            //var workflowAprovaNivelNotificacao = new WorkflowAprovaNivelNotificacao()
            //{
            //    NotificacaoId = notificacao.Id,
            //    WorkflowAprovaNivelId = aprovaNivel.Id
            //};
        }
    }
}