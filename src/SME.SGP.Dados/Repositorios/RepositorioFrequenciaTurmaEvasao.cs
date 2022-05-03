using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFrequenciaTurmaEvasao : IRepositorioFrequenciaTurmaEvasao
    {
        private readonly ISgpContext database;

        public RepositorioFrequenciaTurmaEvasao(ISgpContext database)
        {
            this.database = database;
        }

        public async Task<long> Inserir(FrequenciaTurmaEvasao frequenciaTurmaEvasao)
        {
            return (long)(await database.Conexao.InsertAsync(frequenciaTurmaEvasao));
        }

        public async Task LimparFrequenciaTurmaEvasaoPorTurmasEMeses(long[] turmasIds, int[] meses)
        {
            const string query = @"delete from frequencia_turma_evasao
                                    where turma_id = any(@turmasIds)
                                    and mes = any(@meses)";

            await database.Conexao.ExecuteScalarAsync(query, new { turmasIds, meses });
        }
    }
}
