using Dommel;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioWorkflowAprovaNivelNotificacao : IRepositorioWorkflowAprovacaoNivelNotificacao
    {
        private readonly ISgpContext dataBase;

        public RepositorioWorkflowAprovaNivelNotificacao(ISgpContext dataBase)
        {
            this.dataBase = dataBase ?? throw new System.ArgumentNullException(nameof(dataBase));
        }

        public void Salvar(WorkflowAprovacaoNivelNotificacao workflowAprovaNivelNotificacao)
        {
            dataBase.Conexao.Insert(workflowAprovaNivelNotificacao);
        }
    }
}