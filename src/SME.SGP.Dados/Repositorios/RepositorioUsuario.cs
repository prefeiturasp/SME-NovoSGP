using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Linq;
using System.Text;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioUsuario : RepositorioBase<Usuario>, IRepositorioUsuario
    {
        public RepositorioUsuario(ISgpContext conexao) : base(conexao)
        {
        }

        public Usuario ObterPorCodigoRfLogin(string codigoRf, string login)
        {
            var query = new StringBuilder();
            query.Append("select * from usuario ");
            query.Append("where 1=1 ");

            if (!string.IsNullOrEmpty(codigoRf))
                query.Append("and rf_codigo = @codigoRf ");

            if (!string.IsNullOrEmpty(login))
                query.Append("and login = @login");

            return database.Conexao.Query<Usuario>(query.ToString(), new { codigoRf, login })
                .FirstOrDefault();
        }
    }
}