using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioWorkflowAprovacaoNivel : RepositorioBase<WorkflowAprovacaoNivel>, IRepositorioWorkflowAprovacaoNivel
    {
        public RepositorioWorkflowAprovacaoNivel(ISgpContext conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
        {
                
        }
    }
}
