using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioWorkflowAprovacaoNivel : RepositorioBase<WorkflowAprovacaoNivel>, IRepositorioWorkflowAprovacaoNivel
    {
        public RepositorioWorkflowAprovacaoNivel(ISgpContext conexao) : base(conexao)
        {
                
        }
    }
}
