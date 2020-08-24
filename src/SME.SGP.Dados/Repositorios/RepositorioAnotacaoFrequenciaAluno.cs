using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAnotacaoFrequenciaAluno : RepositorioBase<AnotacaoFrequenciaAluno>, IRepositorioAnotacaoFrequenciaAluno
    {
        public RepositorioAnotacaoFrequenciaAluno(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<bool> ExcluirAnotacoesDaAula(long aulaId)
        {
            var command = "update anotacao_frequencia_aluno set excluido = true where not excluido and aula_id = @aulaId";
            await database.ExecuteAsync(command, new { aulaId });
            return true;
        }

        public async Task<IEnumerable<string>> ListarAlunosComAnotacaoFrequenciaNaAula(long aulaId)
        {
            var query = "select codigo_aluno from anotacao_frequencia_aluno where not excluido and aula_id = @aulaId";

            return await database.Conexao.QueryAsync<string>(query, new { aulaId });
        }

        public async Task<AnotacaoFrequenciaAluno> ObterPorAlunoAula(string codigoAluno, long aulaId)
        {
            var query = "select * from anotacao_frequencia_aluno where not excluido and codigo_aluno = @codigoAluno and aula_id = @aulaId";

            return await database.Conexao.QueryFirstOrDefaultAsync<AnotacaoFrequenciaAluno>(query, new { codigoAluno, aulaId });
        }
    }
}