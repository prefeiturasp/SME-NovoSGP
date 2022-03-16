using Dapper;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioTesteLog : IRepositorioTesteLog
    {
        private readonly ISgpContext conexao;

        public RepositorioTesteLog(ISgpContext conexao)
        {
            this.conexao = conexao ?? throw new ArgumentNullException(nameof(conexao));
        }

        public async Task Gravar(string mensagem)
        {
            var sqlQuery = "insert into tmp_teste_log (mensagem) values (@mensagem);";
            await conexao.ExecuteAsync(sqlQuery, new { mensagem });
        }
    }
}
