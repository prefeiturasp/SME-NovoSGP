using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioDashBoardFrequencia : IRepositorioDashBoardFrequencia
    {
        private readonly ISgpContextConsultas database;

        public RepositorioDashBoardFrequencia(ISgpContextConsultas database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public async Task<IEnumerable<FrequenciaAlunoDashboardDto>> ObterFrequenciasConsolidadasPorTurmaEAno(int anoLetivo, long dreId, long ueId, int modalidade, int semestre, DateTime dataAula, DateTime dataInicioSemana, DateTime datafimSemana, int mes, int tipoPeriodoDashboard, bool visaoDre = false)
        {
            var query = new StringBuilder();

            if (visaoDre)
                query.AppendLine("select dre_abreviacao as Descricao, ");
            else if (ueId == -99)
                query.AppendLine("select turma_ano as Descricao, ");
            else if (ueId != -99 && !visaoDre)
                query.AppendLine("select turma_nome as Descricao, ");


            query.AppendLine(@"tipo as TipoFrequenciaAluno,
                               dre_codigo as DreCodigo,
                               sum(quantidade_presencas) as Presentes,
                               sum(quantidade_remotos) as Remotos,
                               sum(quantidade_ausencias) as Ausentes
                          from consolidado_dashboard_frequencia cdf
                         where ano_letivo = @anoLetivo
                           and modalidade_codigo = @modalidade
                           and tipo = @tipoPeriodo ");

            if (dreId != -99)
                query.AppendLine("and dre_id = @dreId ");

            if (ueId != -99)
                query.AppendLine("and ue_id = @ueId ");

            if (semestre > 0)
                query.AppendLine("and semestre = @semestre ");

            if (tipoPeriodoDashboard == (int)TipoPeriodoDashboardFrequencia.Diario)
                query.AppendLine("and data_aula = @dataAula ");

            if (tipoPeriodoDashboard == (int)TipoPeriodoDashboardFrequencia.Semanal)
                query.AppendLine(@"and data_inicio_semana::date = @dataInicioSemana::date 
                                   and data_fim_semana::date = @dataFimSemana::date ");

            if (tipoPeriodoDashboard == (int)TipoPeriodoDashboardFrequencia.Mensal)
                query.AppendLine(@"and mes = @mes ");

            if (visaoDre)
                query.AppendLine("group by dre_abreviacao, tipo, dre_codigo ");
            else if (ueId == -99)
                query.AppendLine("group by turma_ano, tipo, dre_codigo");
            else if (ueId != -99 && !visaoDre)
                query.AppendLine("group by turma_nome, tipo, dre_codigo");

            return await database.Conexao.QueryAsync<FrequenciaAlunoDashboardDto>(query.ToString(), new
            {
                dreId,
                ueId,
                anoLetivo,
                modalidade,
                semestre,
                dataAula,
                dataInicioSemana,
                datafimSemana,
                mes,
                tipoPeriodo = tipoPeriodoDashboard
            });
        }        

        public async Task<DadosParaConsolidacaoDashBoardFrequenciaDto> ObterDadosParaConsolidacao(int anoLetivo, long turmaId, int modalidade, int tipoPeriodo, DateTime dataAula, DateTime? dataInicioSemana, DateTime? datafimSemana, int? mes)
        {
            var query = new StringBuilder(@"select 
 	                                           count(*) filter(where x.QuantidadePresencas > 0) as Presentes
 	                                         , count(*) filter(where x.QuantidadeRemotos > 0) as Remotos
 	                                         , count(*) filter(where x.QuantidadeAusencias > 0 and x.QuantidadePresencas=0 and x.QuantidadeRemotos=0) as Ausentes 	                            
                                         from
                                             ( 
		                                        select rfa.codigo_aluno,			   
                                                       count(rfa.id) filter(where rfa.valor = 1) as QuantidadePresencas,
			                                           count(rfa.id) filter(where rfa.valor = 2) as QuantidadeAusencias,
                                                       count(rfa.id) filter(where rfa.valor = 3) as QuantidadeRemotos              
                                                  from registro_frequencia_aluno rfa
                                                 inner join registro_frequencia rf on rf.id = rfa.registro_frequencia_id
                                                 inner join aula a on a.id = rf.aula_id
                                                 inner join turma t on t.turma_id = a.turma_id
                                                 inner join ue on ue.id = t.ue_id
                                                 inner join dre on dre.id = ue.dre_id
                                                 where t.ano_letivo = @anoLetivo
                                                   and not rfa.excluido
                                                   and t.modalidade_codigo = @modalidade 
                                                   and t.id = @turmaId ");

            if (tipoPeriodo == (int)TipoPeriodoDashboardFrequencia.Diario)
                query.AppendLine("and a.data_aula = @dataAula ");

            if (tipoPeriodo == (int)TipoPeriodoDashboardFrequencia.Semanal)
                query.AppendLine("and a.data_aula between @dataInicioSemana and @datafimSemana ");

            if (tipoPeriodo == (int)TipoPeriodoDashboardFrequencia.Mensal)
                query.AppendLine(@"and extract(month from a.data_aula) = @mes 
                                   and extract(year from a.data_aula) = @anoLetivo ");

            query.AppendLine(@"group by rfa.codigo_aluno
                               ) as x ");

            var parametros = new
            {
                anoLetivo,
                turmaId,
                modalidade,
                tipoPeriodo,
                dataAula,
                dataInicioSemana,
                datafimSemana,
                mes
            };

            return await database.Conexao.QueryFirstOrDefaultAsync<DadosParaConsolidacaoDashBoardFrequenciaDto>(query.ToString(), parametros);
        }
    }
}
