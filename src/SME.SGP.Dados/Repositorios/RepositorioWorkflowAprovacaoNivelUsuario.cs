using Dommel;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

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

        public async Task SalvarAsync(WorkflowAprovacaoNivelUsuario workflowAprovaNivelUsuario)
        {
            await dataBase.Conexao.InsertAsync(workflowAprovaNivelUsuario);
        }
    }
}