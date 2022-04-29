using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFrequenciaTurmaEvasao : IRepositorioFrequenciaTurmaEvasao
    {
        private readonly ISgpContext _database;

        public RepositorioFrequenciaTurmaEvasao(ISgpContext database)
        {
            _database = database;
        }

        public async Task<long> Inserir(FrequenciaTurmaEvasao frequenciaTurmaEvasao)
        {
            return (long)(await _database.Conexao.InsertAsync(frequenciaTurmaEvasao));
        }

        public async Task LimparFrequenciaTurmaEvasaoPorTurmas(long[] turmasIds)
        {
            const string query = @"delete from frequencia_turma_evasao
                                    where turma_id = any(@turmaIds)";

            await _database.Conexao.ExecuteScalarAsync(query, new { turmasIds });
        }
    }
}
