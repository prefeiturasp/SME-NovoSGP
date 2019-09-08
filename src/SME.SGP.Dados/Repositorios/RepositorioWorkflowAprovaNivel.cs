using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioWorkflowAprovaNivel : RepositorioBase<WorkflowAprovaNivel>, IRepositorioWorkflowAprovaNivel
    {
        public RepositorioWorkflowAprovaNivel(ISgpContext conexao) : base(conexao)
        {
        }
    }
}