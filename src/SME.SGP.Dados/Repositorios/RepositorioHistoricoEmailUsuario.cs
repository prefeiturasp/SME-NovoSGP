using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioHistoricoEmailUsuario : RepositorioBase<HistoricoEmailUsuario>, IRepositorioHistoricoEmailUsuario
    {
        public RepositorioHistoricoEmailUsuario(ISgpContext conexao) : base(conexao)
        {
        }
    }
}
