using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.Frequencia;
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

        public async Task<IEnumerable<FrequenciaAlunoDashboardDto>> ObterFrequenciasDiariaConsolidadas(int anoLetivo, long dreId, long ueId, int modalidade, int semestre, string anoTurma, DateTime dataAula, bool visaoDre = false)
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

        public async Task<IEnumerable<DadosParaConsolidacaoDashBoardFrequenciaDto>> ObterDadosParaConsolidacao(int anoLetivo, long turmaId, int modalidade, DateTime dataAula)
        {
           var query = new StringBuilder($@"with totalAulas as (
                                                select
                                                    t.id as TurmaId,
                                                    a.data_aula as DataAula, 
                                                    sum(a.quantidade) as TotalAulas
                                                from aula a
                                                join turma t on t.turma_id = a.turma_id
                                                where
                                                    not a.excluido
                                                    and t.id = @turmaId 
                                                    and a.data_aula = @dataAula
                                                group by t.id,a.data_aula
                                            ),
                                            totalFrequencia as (
                                            select
                                                agrupamento_final.codigo_aluno CodigoAluno,                                                    
                                                agrupamento_final.TotalFrequencias,    
                                                count(*) filter(where agrupamento_final.QuantidadePresencas > 0) as Presentes,
                                                count(*) filter(where agrupamento_final.QuantidadeRemotos > 0) as Remotos,
                                                count(*) filter(where agrupamento_final.QuantidadeAusencias > 0) as Ausentes
                                            from
                                                (
                                                select
                                                    agrupamento_por_aluno.codigo_aluno,                                                    
                                                    case when agrupamento_por_aluno.QuantidadeAusencias > 0 then 
                                                             case when agrupamento_por_aluno.QuantidadeRemotos > 0 then 0
                                                            else 
                                                                case when agrupamento_por_aluno.QuantidadePresencas > 0 then 0
                                                                else agrupamento_por_aluno.QuantidadeAusencias
                                                                end
                                                            end
                                                    else 0
                                                    end QuantidadeAusencias,
                                                    case when agrupamento_por_aluno.QuantidadeRemotos > 0 then 
                                                             case when agrupamento_por_aluno.QuantidadePresencas > 0 then 0
                                                            else agrupamento_por_aluno.QuantidadeRemotos
                                                            end
                                                    else 0
                                                    end QuantidadeRemotos,
                                                    agrupamento_por_aluno.QuantidadePresencas,
                                                    (agrupamento_por_aluno.QuantidadePresencas + agrupamento_por_aluno.QuantidadeAusencias + agrupamento_por_aluno.QuantidadeRemotos) as TotalFrequencias
                                                from
                                                    (
                                                    select
                                                        rfa.codigo_aluno,
                                                        count(rfa.id) filter(where rfa.valor = 1) as QuantidadePresencas,
                                                        count(rfa.id) filter(where rfa.valor = 2) as QuantidadeAusencias,
                                                        count(rfa.id) filter(where rfa.valor = 3) as QuantidadeRemotos
                                                    from
                                                        registro_frequencia_aluno rfa
                                                    join aula a on a.id = rfa.aula_id
                                                    join turma t on t.turma_id = a.turma_id
                                                    join ue on ue.id = t.ue_id
                                                    join dre on dre.id = ue.dre_id
                                                    where
                                                        t.ano_letivo = @anoLetivo
                                                        and not rfa.excluido
                                                        and t.modalidade_codigo = @modalidade
                                                        and t.id = @turmaId 
                                                        and a.data_aula = @dataAula
                                                    group by
                                                        rfa.codigo_aluno            
                                            ) as agrupamento_por_aluno
                                            ) as agrupamento_final
                                            group by
                                                agrupamento_final.codigo_aluno,                                                
                                                agrupamento_final.TotalFrequencias
                                            )
                                            select 
                                            codigoAluno, DataAula, Presentes, Ausentes,remotos,totalAulas, totalFrequencias
                                            from
                                                totalAulas
                                                left join totalFrequencia on 1 = 1
                                            order by codigoAluno ");

            var parametros = new
            {
                anoLetivo,
                turmaId,
                modalidade,
                dataAula
            };

            return await database.Conexao.QueryAsync<DadosParaConsolidacaoDashBoardFrequenciaDto>(query.ToString(), parametros);
        }

        public async Task<IEnumerable<DadosParaConsolidarFrequenciaDiariaAlunoDto>> ObterDadosParaConsolidacaoPainelEducacional(int anoLetivo)
        {
            var query = @"Select 
                               dre_codigo as CodigoDre,
                               ue_id as UeId,
                               turma_id as TurmaId,
                               turma_nome as NomeTurma,
                               ano_letivo as AnoLetivo,
                               data_aula as DataAula,
                               sum(quantidade_presencas) as TotalPresentes,
                               total_frequencias as TotalFrequencias, 
                               sum(quantidade_remotos) as TotalRemotos,
                               sum(quantidade_ausencias) as TotalAusentes
                          from consolidado_dashboard_frequencia cdf
                          where ano_letivo = @anoLetivo
                          group by dre_codigo,
                          	   ano_letivo,
                               turma_id,
                               ue_id,
                               turma_nome,
                               total_frequencias,
                               data_aula";

            var parametros = new
            {
                anoLetivo
            };

            return await database.Conexao.QueryAsync<DadosParaConsolidarFrequenciaDiariaAlunoDto>(query.ToString(), parametros);
        }
    }
}
