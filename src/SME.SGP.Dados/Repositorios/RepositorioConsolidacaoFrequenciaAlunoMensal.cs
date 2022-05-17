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
            string query = @"delete from consolidacao_frequencia_aluno_mensal
                                    where turma_id in (#turmaIds)
                                    and mes in (#meses)";

            query = query.Replace("#turmaIds", string.Join(",", turmaIds));
            query = query.Replace("#meses", string.Join(",", meses));

            await database.Conexao.ExecuteAsync(query, new { turmaIds, meses });
        }

        public async Task<IEnumerable<ConsolidacaoFrequenciaAlunoMensalDto>> ObterConsolidacoesFrequenciaAlunoMensalPorTurmaEMes(long turmaId, int mes)
        {
            const string query = @"select turma_id as TurmaId, 
                                        aluno_codigo as AlunoCodigo,
                                        mes,
                                        percentual,
                                        quantidade_aulas as QuantidadeAulas,
                                        quantidade_ausencias as QuantidadeAusencias,
                                        quantidade_compensacoes as QuantidadeCompensacoes
                                    from consolidacao_frequencia_aluno_mensal
                                    where turma_id = @turmaId 
                                    and mes = @mes";

            return await database.Conexao.QueryAsync<ConsolidacaoFrequenciaAlunoMensalDto>(query, new { turmaId, mes });
        }
    }
}
