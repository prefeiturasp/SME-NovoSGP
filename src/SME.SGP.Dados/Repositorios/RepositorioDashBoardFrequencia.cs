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

        public async Task<IEnumerable<FrequenciaAlunoDashboardDto>> ObterFrequenciasConsolidadasPorTurmaEAno(int anoLetivo, long dreId, long ueId, int modalidade, int semestre, string anoTurma, DateTime dataAula, DateTime dataInicioSemana, DateTime datafimSemana, int mes, int tipoPeriodoDashboard, bool visaoDre = false)
        {
            var query = new StringBuilder();            
            var anoTurmaModalidade = "";

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

            if (!string.IsNullOrEmpty(anoTurma) && !anoTurma.Contains("-99"))
            {
                var modalidadeEnum = ((Modalidade)modalidade);
                anoTurmaModalidade = $"{modalidadeEnum.ShortName()}-{anoTurma}";
                query.AppendLine("and turma_ano = @anoTurma ");
            }

            if (tipoPeriodoDashboard == (int)TipoPeriodoDashboardFrequencia.Diario)
                query.AppendLine("and data_aula = @dataAula ");

            if (tipoPeriodoDashboard == (int)TipoPeriodoDashboardFrequencia.Semanal)
                query.AppendLine(@"and data_inicio_semana::date = @dataInicioSemana::date ");

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
                anoTurma = anoTurmaModalidade,
                dataAula,
                dataInicioSemana,
                datafimSemana,
                mes,
                tipoPeriodo = tipoPeriodoDashboard
            });
        }

        public async Task<IEnumerable<DadosParaConsolidacaoDashBoardFrequenciaDto>> ObterDadosParaConsolidacao(int anoLetivo, long turmaId, int modalidade, DateTime dataInicioSemana, DateTime datafimSemana, DateTime? dataAula = null)
        {
            var filtroData = dataAula.HasValue
                ? " and a.data_aula = @dataAula "
                : " and a.data_aula between @dataInicioSemana and @datafimSemana ";
            
            var query = new StringBuilder($@"select 
                                                 agrupamento_final.codigo_aluno CodigoAluno,                                              
                                                 agrupamento_final.data_aula DataAula,     
                                                 count(*) filter(where agrupamento_final.QuantidadePresencas > 0) as Presentes,
                                                 count(*) filter(where agrupamento_final.QuantidadeRemotos > 0) as Remotos,
                                                 count(*) filter(where agrupamento_final.QuantidadeAusencias > 0) as Ausentes   
                                            from (
                                            select 
                                                 agrupamento_por_aluno_data.codigo_aluno, agrupamento_por_aluno_data.data_aula,
                                                 case when agrupamento_por_aluno_data.QuantidadeAusencias > 0 then 
     		                                            case when agrupamento_por_aluno_data.QuantidadeRemotos > 0 then 	0 
     			                                            else 
				                                                 case when agrupamento_por_aluno_data.QuantidadePresencas > 0 then 0
				                                                    else agrupamento_por_aluno_data.QuantidadeAusencias
		     		                                             end
     		                                            end
                                                 else 0 end QuantidadeAusencias,
                                                 case when agrupamento_por_aluno_data.QuantidadeRemotos > 0 then 
     		                                            case when agrupamento_por_aluno_data.QuantidadePresencas > 0 then 0
				                                            else agrupamento_por_aluno_data.QuantidadeRemotos
		                                                end     		
                                                 else 0 end   QuantidadeRemotos,
                                                 agrupamento_por_aluno_data.QuantidadePresencas
                                             from
                                                 (
                                                    select rfa.codigo_aluno, a.data_aula,			   
                                                           count(rfa.id) filter(where rfa.valor = 1) as QuantidadePresencas,
                                                           count(rfa.id) filter(where rfa.valor = 2) as QuantidadeAusencias,
                                                           count(rfa.id) filter(where rfa.valor = 3) as QuantidadeRemotos               
                                                      from registro_frequencia_aluno rfa
         	                                            join aula a on a.id = rfa.aula_id
	                                                    join turma t on t.turma_id = a.turma_id
    	                                                join ue on ue.id = t.ue_id
        	                                            join dre on dre.id = ue.dre_id 
                                                     where t.ano_letivo = @anoLetivo
                                                       and not rfa.excluido
                                                       and t.modalidade_codigo = @modalidade 
                                                       and t.id = @turmaId 
                                                       {filtroData}                                                        
                                            group by rfa.codigo_aluno, a.data_aula
                                            ) as agrupamento_por_aluno_data
                                            ) as agrupamento_final
                                            group by agrupamento_final.codigo_aluno, agrupamento_final.data_aula ");

            var parametros = new
            {
                anoLetivo,
                turmaId,
                modalidade,
                dataAula,
                dataInicioSemana,
                datafimSemana
            };

            return await database.Conexao.QueryAsync<DadosParaConsolidacaoDashBoardFrequenciaDto>(query.ToString(), parametros);
        }
    }
}
