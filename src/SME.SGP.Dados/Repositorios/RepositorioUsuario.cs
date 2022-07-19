using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioUsuario : RepositorioBase<Usuario>, IRepositorioUsuario
    {
        public RepositorioUsuario(ISgpContext conexao, IServicoMensageria servicoMensageria) : base(conexao, servicoMensageria)
        {
        }

        public async Task AtualizarUltimoLogin(long id, DateTime ultimoLogin)
        {
            var comando = "update usuario set ultimo_login = @ultimoLogin where id = @id";
            await database.Conexao.ExecuteScalarAsync(comando, new { id, ultimoLogin });
        }
    }
}