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
            var query = @$"select id as Id,
                                        turma_id as TurmaId, 
                                        aluno_codigo as AlunoCodigo,
                                        mes,
                                        percentual,
                                        quantidade_aulas as QuantidadeAulas,
                                        quantidade_ausencias as QuantidadeAusencias,
                                        quantidade_compensacoes as QuantidadeCompensacoes
                                    from consolidacao_frequencia_aluno_mensal
                                    where turma_id = @turmaId 
                                    {(mes != 0 ? " and mes = @mes" : string.Empty)}";
                
            return await database.Conexao.QueryAsync<ConsolidacaoFrequenciaAlunoMensalDto>(query, new { turmaId, mes });
        }

        public async Task AlterarConsolidacaoAluno(long consolidacaoId, double percentual, int quantidadeAulas, int quantidadeAusencias, int quantidadeCompensacoes)
        {
            string query = @"update consolidacao_frequencia_aluno_mensal 
                                    set percentual = @percentual, 
                                    quantidade_aulas = @quantidadeAulas,
                                    quantidade_ausencias = @quantidadeAusencias,
                                    quantidade_compensacoes = @quantidadeCompensacoes
                                    where id = @consolidacaoId";

            await database.Conexao.ExecuteAsync(query, new { consolidacaoId, percentual, quantidadeAulas, quantidadeAusencias, quantidadeCompensacoes });
        }

        public async Task RemoverConsolidacaoAluno(long consolidacaoId)
        {
            string query = @"delete from consolidacao_frequencia_aluno_mensal where id = @consolidacaoId";
            await database.Conexao.ExecuteAsync(query, new { consolidacaoId });
        }
    }
}
