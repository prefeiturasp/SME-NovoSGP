using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Text;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioUsuario : RepositorioBase<Usuario>, IRepositorioUsuario
    {
        public RepositorioUsuario(ISgpContext conexao) : base(conexao)
        {
        }

        public bool ExisteUsuarioComMesmoEmail(string email, long idUsuarioExistente)
        {
            var query = new StringBuilder();
            query.Append("select * from usuario ");
            query.Append("where email = @email and id <> @id");

            return database.Conexao.Query<Usuario>(query.ToString(), new { email, id = idUsuarioExistente })
                .Any();
        }

        public Usuario ObterPorCodigoRfLogin(string codigoRf, string login)
        {
            var query = new StringBuilder();
            query.AppendLine("select * from usuario");
            query.AppendLine("where 1=1");

            if (!string.IsNullOrEmpty(codigoRf))
                query.AppendLine("and rf_codigo = @codigoRf");

            if (!string.IsNullOrEmpty(login))
                query.AppendLine("and login = @login");

            return database.Conexao.Query<Usuario>(query.ToString(), new { codigoRf, login })
                .FirstOrDefault();
        }

        public Usuario ObterPorTokenRecuperacaoSenha(Guid token)
        {
            var query = new StringBuilder();
            query.Append("select * from usuario ");
            query.Append("where token_recuperacao_senha = @token ");

            return database.Conexao.Query<Usuario>(query.ToString(), new { token })
                .FirstOrDefault();
        }
    }
}