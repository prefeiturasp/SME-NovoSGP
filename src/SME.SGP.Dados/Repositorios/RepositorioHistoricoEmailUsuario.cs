using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioHistoricoEmailUsuario : RepositorioBase<HistoricoEmailUsuario>, IRepositorioHistoricoEmailUsuario
    {
        public RepositorioHistoricoEmailUsuario(ISgpContext conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
        {
        }
    }
}
