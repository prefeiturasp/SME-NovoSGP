using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioSuporteUsuario : RepositorioBase<SuporteUsuario>, IRepositorioSuporteUsuario
    {
        public RepositorioSuporteUsuario(ISgpContext conexao) : base(conexao)
        {
        }
    }
}
