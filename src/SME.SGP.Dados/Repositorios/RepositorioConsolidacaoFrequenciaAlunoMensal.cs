using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConsolidacaoFrequenciaAlunoMensal : IRepositorioConsolidacaoFrequenciaAlunoMensal
    {
        private readonly ISgpContext database;

        public RepositorioConsolidacaoFrequenciaAlunoMensal(ISgpContext database)
        {
            this.database = database;
        }

        public async Task<long> Inserir(ConsolidacaoFrequenciaAlunoMensal consolidacao)
        {
            return (long)(await database.Conexao.InsertAsync(consolidacao));
        }

        public async Task LimparConsolidacaoFrequenciasAlunosPorTurmasEMeses(long[] turmaIds, int[] meses)
        {
            const string query = @"delete from consolidacao_frequencia_aluno_mensal
                                    where turma_id = any(@turmaIds)
                                    and mes = any(@meses)";

            await database.Conexao.ExecuteScalarAsync(query, new { turmaIds, meses });
        }

        public async Task<IEnumerable<ConsolidacaoFrequenciaAlunoMensalDto>> ObterConsolidacoesFrequenciaAlunoMensalPorTurmaEMes(long turmaId, int mes)
        {
            const string query = @"select * from consolidacao_frequencia_aluno_mensal
                                    where turma_id = @turmaId 
                                    and mes = @mes";

            return await database.Conexao.QueryAsync<ConsolidacaoFrequenciaAlunoMensalDto>(query, new { turmaId, mes });
        }
    }
}
