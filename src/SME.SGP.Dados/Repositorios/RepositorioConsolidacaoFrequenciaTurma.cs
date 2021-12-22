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

        public async Task ExcluirConsolidacaoDashBoard(int anoLetivo, long turmaId, DateTime dataAula, DateTime? dataInicioSemanda, DateTime? dataFinalSemena, int? mes, TipoPeriodoDashboardFrequencia tipoPeriodo)
        {
            var query = new StringBuilder(@"delete 
                                              from consolidado_dashboard_frequencia
                                             where turma_id = @turmaId
                                               and tipo = @tipoPeriodo
                                               and ano_letivo = @anoLetivo ");

            if (tipoPeriodo == TipoPeriodoDashboardFrequencia.Diario)
                query.AppendLine("and data_aula::date = @dataAula ");

            if (tipoPeriodo == TipoPeriodoDashboardFrequencia.Semanal)
                query.AppendLine(@"and data_inicio_semana::date = @dataInicioSemanda
                                   and data_fim_semana::date = @dataFinalSemena ");

            if (tipoPeriodo == TipoPeriodoDashboardFrequencia.Mensal)
                query.AppendLine("and mes = @mes ");

            var parametros = new
            {
                anoLetivo,
                turmaId,
                dataAula,
                dataInicioSemanda,
                dataFinalSemena,
                mes,
                tipoPeriodo
            };

            await database.Conexao.ExecuteScalarAsync(query.ToString(), parametros);
        }
    }
}