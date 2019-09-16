using Dommel;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioWorkflowAprovacaoNivelUsuario : IRepositorioWorkflowAprovacaoNivelUsuario
    {
        private readonly ISgpContext dataBase;

        public RepositorioWorkflowAprovacaoNivelUsuario(ISgpContext dataBase)
        {
            this.dataBase = dataBase ?? throw new System.ArgumentNullException(nameof(dataBase));
        }

        public void Salvar(WorkflowAprovacaoNivelUsuario workflowAprovaNivelUsuario)
        {
            dataBase.Conexao.Insert(workflowAprovaNivelUsuario);
        }
    }
}