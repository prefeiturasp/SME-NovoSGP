using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConsolidacaoFrequenciaAlunoMensal : IRepositorioConsolidacaoFrequenciaAlunoMensal
    {
        private readonly ISgpContext _database;

        public RepositorioConsolidacaoFrequenciaAlunoMensal(ISgpContext database)
        {
            _database = database;
        }

        public async Task<long> Inserir(ConsolidacaoFrequenciaAlunoMensal consolidacao)
        {
            return (long)(await _database.Conexao.InsertAsync(consolidacao));
        }

        public async Task LimparConsolidacaoFrequenciasAlunosPorTurmasEMeses(long[] turmaIds, int[] meses)
        {
            const string query = @"delete from consolidacao_frequencia_aluno_mensal
                                    where turma_id = any(@turmaIds)
                                    and mes = any(@meses)";

            await _database.Conexao.ExecuteScalarAsync(query, new { turmaIds, meses });
        }
    }
}
