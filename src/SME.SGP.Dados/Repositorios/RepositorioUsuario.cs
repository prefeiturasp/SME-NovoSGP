using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Linq;


namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioUsuario : RepositorioBase<Usuario>, IRepositorioUsuario
    {
        public RepositorioUsuario(ISgpContext conexao) : base(conexao)
        {

        }

        public Usuario ObterPorCodigoRf(string codigoRf)
        {
            return database.Conexao.Query<Usuario>("select * from usuario where rf_codigo = @codigoRf", new { codigoRf })
                .FirstOrDefault();
        }
    }
}
