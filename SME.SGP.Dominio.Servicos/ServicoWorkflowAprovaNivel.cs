using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoWorkflowAprovaNivel : IServicoWorkflowAprovaNivel
    {
        private readonly IRepositorioNotificacao repositorioNotificacao;
        private readonly IRepositorioWorkflowAprovacao repositorioWorkflowAprovacao;
        private readonly IUnitOfWork unitOfWork;

        public ServicoWorkflowAprovaNivel(IRepositorioWorkflowAprovacao repositorioWorkflowAprovacao, IUnitOfWork unitOfWork,
            IRepositorioNotificacao repositorioNotificacao)
        {
            this.repositorioWorkflowAprovacao = repositorioWorkflowAprovacao ?? throw new System.ArgumentNullException(nameof(repositorioWorkflowAprovacao));
            this.unitOfWork = unitOfWork ?? throw new System.ArgumentNullException(nameof(unitOfWork));
            this.repositorioNotificacao = repositorioNotificacao ?? throw new System.ArgumentNullException(nameof(repositorioNotificacao));
        }

        public void ConfiguracaoInicial(string codigo)
        {
            var workflowAprovaNiveis = repositorioWorkflowAprovacao.ObterNiveisPorCodigo(codigo);

            if (workflowAprovaNiveis == null || workflowAprovaNiveis.Count() == 0)
                throw new NegocioException($"Não foi possível localizar o workflow de aprovação de nível com o código: {codigo}");

            //var primeiroNivel = workflowAprovaNiveis.OrderBy(a => a.Nivel)
            //    .FirstOrDefault()
            //    .Nivel;

            //var primeirosNiveis = workflowAprovaNiveis
            //    .Where(a => a.Nivel == primeiroNivel)
            //    .ToList();

            //EnviaNotificacaoParaNiveis(primeirosNiveis);
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