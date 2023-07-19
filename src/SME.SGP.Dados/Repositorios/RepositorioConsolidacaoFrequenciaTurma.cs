using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConsolidacaoFrequenciaTurma : IRepositorioConsolidacaoFrequenciaTurma
    {
        private readonly ISgpContext database;

        public RepositorioConsolidacaoFrequenciaTurma(ISgpContext database)
        {
            this.database = database;
        }

        public async Task<long> Inserir(ConsolidacaoFrequenciaTurma consolidacao)
        {
            return (long)(await database.Conexao.InsertAsync(consolidacao));
        }

        public async Task LimparConsolidacaoFrequenciasTurmasPorAno(int ano)
        {
            var query = @"delete from consolidacao_frequencia_turma
                        where turma_id in (
                            select id from turma where ano_letivo = @ano)";

            await database.Conexao.ExecuteScalarAsync(query, new { ano });
        }

        public async Task<long> InserirConsolidacaoDashBoard(ConsolidacaoDashBoardFrequencia consolidacao)
        {
            return (long)(await database.Conexao.InsertAsync(consolidacao));
        }

        public async Task<RetornoConsolidacaoExistenteDto> ObterConsolidacaoDashboardPorTurmaAnoTipoPeriodoMes(long turmaId, int anoLetivo, TipoPeriodoDashboardFrequencia tipo, DateTime dataAula, int? mes, DateTime? dataInicioSemana, DateTime? dataFimSemana)
        {
            var query = new StringBuilder(@"select cdf.id as Id, 
                                                cdf.turma_id as TurmaId,
                                                cdf.quantidade_presencas as Presentes, 
                                                cdf.quantidade_ausencias as Ausentes, 
                                                cdf.quantidade_remotos as Remotos
                                                from consolidado_dashboard_frequencia cdf 
                                                where cdf.turma_id = @turmaId 
                                                and cdf.ano_letivo = @anoLetivo 
                                                and cdf.tipo = @tipo");

            if (tipo == TipoPeriodoDashboardFrequencia.Diario)
                query.AppendLine(" and cdf.data_aula = @dataAula");

            if (tipo == TipoPeriodoDashboardFrequencia.Semanal && dataFimSemana != null && dataInicioSemana != null)
                query.AppendLine(@" and cdf.data_inicio_semana = @dataInicioSemana
                                    and cdf.data_fim_semana = @dataFimSemana");

            if (tipo == TipoPeriodoDashboardFrequencia.Mensal && mes != null)
                query.AppendLine(" and cdf.mes = @mes");

            return await database.Conexao.QueryFirstOrDefaultAsync<RetornoConsolidacaoExistenteDto>(query.ToString(), new { turmaId, anoLetivo, tipo, dataAula, mes, dataInicioSemana, dataFimSemana});
        }

        public async Task AlterarConsolidacaoDashboardTurmaMesPeriodoAno(long id, int quantidadePresente, int quantidadeAusente, int quantidadeRemoto)
        {
            string query = @"update consolidado_dashboard_frequencia 
                                    set quantidade_presencas = @quantidadePresente, 
                                    quantidade_ausencias = @quantidadeAusente,
                                    quantidade_remotos = @quantidadeRemoto
                                    where id = @id";

            await database.Conexao.ExecuteAsync(query, new { id, quantidadePresente, quantidadeAusente, quantidadeRemoto});
        }

        public async Task Excluir(long turmaId)
        {
            var query = "delete from consolidacao_frequencia_turma where turma_id = @turmaId;";
            await database.Conexao.ExecuteAsync(query, new { turmaId });
        }
    }
}