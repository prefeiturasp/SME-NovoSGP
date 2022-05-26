using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioSuporteUsuario : IRepositorioSuporteUsuario
    {
        private ISgpContext database;
        public RepositorioSuporteUsuario(ISgpContext conexao) 
        {
            database = conexao;
        }

        public long Salvar(SuporteUsuario entidade)
        {
            return (long)database.Conexao.Insert(entidade);
        }
    }
}
