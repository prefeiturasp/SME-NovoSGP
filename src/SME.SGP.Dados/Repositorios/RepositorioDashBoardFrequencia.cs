using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<FrequenciaAlunoDashboardDto>> ObterFrequenciasDiariaConsolidadas(int anoLetivo, long dreId, long ueId, int modalidade, int semestre, string anoTurma, DateTime dataAula, bool visaoDre)
        {
            var selectSQL = string.Empty;
            var groupBySQL = string.Empty;

            if (visaoDre)
            {
                selectSQL = "select dre_abreviacao as Descricao, ";
                groupBySQL = "group by dre_abreviacao, tipo, dre_codigo ";
            }
            else if (ueId == -99)
            {
                selectSQL = "select turma_ano as Descricao, ";
                groupBySQL = "group by turma_ano, tipo, dre_codigo ";
            }
            else if (ueId != -99 && !visaoDre)
            {
                selectSQL = "select turma_nome as Descricao, ";
                groupBySQL = "group by turma_nome, tipo, dre_codigo ";
            }
                
            groupBySQL += ", total_aulas, total_frequencias ";

            selectSQL += @"    dre_codigo as DreCodigo,
                               total_aulas as TotalAulas,
                               total_frequencias as TotalFrequencias, 
                               sum(quantidade_presencas) as Presentes,
                               sum(quantidade_remotos) as Remotos,
                               sum(quantidade_ausencias) as Ausentes
                          from consolidado_dashboard_frequencia cdf
                            join turma t on t.id = cdf.turma_id 
                            join ue on ue.id = t.ue_id 
                            join dre on dre.id = ue.dre_id 
                         where t.ano_letivo = @anoLetivo
                           and t.modalidade_codigo = @modalidade
                           and cdf.tipo = @tipoPeriodo 
                           and cdf.data_aula = @dataAula ";

            if (dreId != -99)
                selectSQL += "and dre.id = @dreId ";

            if (ueId != -99)
                selectSQL += "and ue.id = @ueId ";

            if (semestre > 0)
                selectSQL += "and t.semestre = @semestre ";

            if (anoTurma != "-99")
                selectSQL += "and t.ano = @anoTurma ";

            selectSQL += groupBySQL;

            return await database.Conexao.QueryAsync<FrequenciaAlunoDashboardDto>(selectSQL, new
            {
                dreId,
                ueId,
                anoLetivo,
                modalidade,
                semestre,
                anoTurma,
                dataAula,
                tipoPeriodo = (int)TipoPeriodoDashboardFrequencia.Diario
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
                                                 agrupamento_final.totalAulas,
                                                 agrupamento_final.totalFrequencias,  
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
                                                 agrupamento_por_aluno_data.QuantidadePresencas,
                                                 agrupamento_por_aluno_data.totalAulas,
                                                 agrupamento_por_aluno_data.totalFrequencias
                                             from
                                                 (
                                                    select rfa.codigo_aluno, a.data_aula,			   
                                                           count(rfa.id) filter(where rfa.valor = 1) as QuantidadePresencas,
                                                           count(rfa.id) filter(where rfa.valor = 2) as QuantidadeAusencias,
                                                           count(rfa.id) filter(where rfa.valor = 3) as QuantidadeRemotos,
                                                           count(a.id)  as totalAulas,
                                                           count(rfa.id)  as totalFrequencias
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
                                            group by agrupamento_final.codigo_aluno, 
                                                     agrupamento_final.data_aula,
                                                     agrupamento_final.totalAulas,
                                                     agrupamento_final.totalFrequencias");

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
