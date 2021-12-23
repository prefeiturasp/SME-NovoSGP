using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioUsuarioConsulta : RepositorioBase<Usuario>, IRepositorioUsuarioConsulta
    {
        public RepositorioUsuarioConsulta(ISgpContextConsultas conexao) : base(conexao)
        {
        }

        public async Task<Usuario> ObterPorCodigoRfLogin(string codigoRf, string login)
        {
            var query = new StringBuilder();
            query.AppendLine("select * from usuario");
            query.AppendLine("where 1=1");

            if (!string.IsNullOrEmpty(codigoRf))
                query.AppendLine("and rf_codigo = @codigoRf");

            if (!string.IsNullOrEmpty(login))
                query.AppendLine("and login = @login");
            else
                query.AppendLine("or login = @codigoRf");

            var usuarios = await database.Conexao.QueryAsync<Usuario>(query.ToString(), new { codigoRf, login });

            return usuarios.FirstOrDefault();
        }

        public async Task<Usuario> ObterPorCodigoRfLoginAsync(string codigoRf, string login)
        {
            var query = new StringBuilder();
            query.AppendLine("select * from usuario");
            query.AppendLine("where 1=1");

            if (!string.IsNullOrEmpty(codigoRf))
                query.AppendLine("and rf_codigo = @codigoRf");

            if (!string.IsNullOrEmpty(login))
                query.AppendLine("and login = @login");
            else
                query.AppendLine("or login = @codigoRf");

            var usuarios = await database.Conexao.QueryAsync<Usuario>(query.ToString(), new { codigoRf, login });
            return usuarios.FirstOrDefault();
        }

        public async Task<Usuario> ObterUsuarioPorCodigoRfAsync(string codigoRf)
        {
            var query = new StringBuilder();
            query.AppendLine("select * from usuario");
            query.AppendLine("where rf_codigo = @codigoRf");

            return await database.Conexao.QueryFirstOrDefaultAsync<Usuario>(query.ToString(), new { codigoRf });
        }

        public async Task<Usuario> ObterPorTokenRecuperacaoSenha(Guid token)
        {
            var retorno = await database.Conexao.QueryAsync<Usuario>("select * from usuario where token_recuperacao_senha = @token ", new { token });
            return retorno.FirstOrDefault();
        }

        public async Task<ProfessorDto> ObterProfessorDaTurmaPorAulaId(long aulaId)
        {
            var query = @"select u.id, 
                                 u.rf_codigo as codigoRf, 
                                 u.nome 
                            from aula a
                          inner join usuario u on a.professor_rf = u.rf_codigo 
                          where a.id = @aulaId";
            return await database.Conexao.QueryFirstOrDefaultAsync<ProfessorDto>(query.ToString(), new { aulaId });
        }

        public async Task<IEnumerable<long>> ObterUsuariosIdPorCodigoRf(IList<string> codigoRf)
        {
            var query = new StringBuilder();
            query.Append("select id from usuario ");
            query.Append("where rf_codigo in ");
            query.Append("(");
            foreach(var rf in codigoRf)
            {
                query.Append($"'{rf}',");
            }
            query.Append("'0')");
            return await database.Conexao.QueryAsync<long>(query.ToString());
        }

        public async Task<IEnumerable<Usuario>> ObterUsuariosPorCodigoRf(IList<string> codigosRf)
        {
            var query = @"select 
                            * 
                        from 
                            usuario u
                        where
                            u.rf_codigo = any(@codigosRf)";
            return await database.Conexao.QueryAsync<Usuario>(query.ToString(), new { codigosRf });
        }

        public async Task<long> ObterUsuarioIdPorCodigoRfAsync(string codigoRf)
        {
            var query = @"select id 
                            from usuario 
                           where rf_codigo = @codigoRf";

            return await database.Conexao.QueryFirstOrDefaultAsync<long>(query, new { codigoRf });
        }

        public async Task<long> ObterUsuarioIdPorLoginAsync(string login)
        {
            var query = @"select id 
                            from usuario 
                           where login = @login";

            return await database.Conexao.QueryFirstOrDefaultAsync<long>(query, new { login });
        }
    }
}