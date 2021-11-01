using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioConsolidacaoRegistrosPedagogicos : IRepositorioConsolidacaoRegistrosPedagogicos
    {
        private readonly ISgpContext database;

        public RepositorioConsolidacaoRegistrosPedagogicos(ISgpContext database)
        {
            this.database = database ?? throw new System.ArgumentNullException(nameof(database));
        }

        public async Task<bool> ExisteConsolidacaoRegistroPedagogicoPorAno(int ano)
        {
            var query = @"select 1 
                          from consolidacao_registros_pedagogicos c
                         inner join turma t on t.id = c.turma_id
                         where t.ano_letivo = @ano";

            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(query, new { ano });
        }

        public async Task ExcluirPorAno(int anoLetivo)
        {
            var query = @"delete from consolidacao_registros_pedagogicos where ano_letivo = @anoLetivo";

            await database.Conexao.ExecuteScalarAsync(query, new { anoLetivo });
        }


        public async Task<IEnumerable<ConsolidacaoRegistrosPedagogicosDto>> GerarRegistrosPedagogicos(long ueId, int anoLetivo)
        {
            var query = @"select 					      
                              pe.id as PeriodoEscolarId 
							, pe.bimestre as Bimestre
							, t.id as TurmaId
							, t.turma_id as TurmaCodigo
                            , t.ano_letivo as AnoLetivo
                            , a.disciplina_id as ComponenteCurricularId
                            , count(a.id) as QuantidadeAulas
                            , count(a.id) filter (where rf.id is null) as FrequenciasPendentes
                            , max(rf.criado_em) as DataUltimaFrequencia
                            , max(pa.criado_em) as DataUltimoPlanoAula
                            , max(db.criado_em) as DataUltimoDiarioBordo
                            , case 
                              when max(pa.criado_em) is not null or disciplina_id != '512' then 0
                              else count(a.id) filter (where db.id is null)
                              end as DiarioBordoPendentes
                            , case 
                              when max(db.criado_em) is not null or disciplina_id = '512' then 0
                              else count(a.id) filter (where pa.id is null)
                              end as PlanoAulaPendentes
                            , a.professor_rf as RFProfessor
                            , t.modalidade_codigo as ModalidadeCodigo
                          from aula a
                          	 left join diario_bordo db on db.aula_id = a.id and not db.excluido 
	                         inner join turma t on t.turma_id = a.turma_id 
	                         inner join periodo_escolar pe on pe.tipo_calendario_id = a.tipo_calendario_id 
	                         			and a.data_aula between pe.periodo_inicio and pe.periodo_fim
	                         left join registro_frequencia rf on rf.aula_id = a.id and not rf.excluido 
	                         left join plano_aula pa on pa.aula_id  = a.id and not pa.excluido 
                         where not a.excluido 
                           and t.ue_id = @ueId
                           and t.ano_letivo = @anoLetivo
                        group by pe.id, pe.bimestre, t.id, a.disciplina_id, db.criado_em, pa.criado_em, a.professor_rf, t.modalidade_codigo";

            return await database.Conexao.QueryAsync<ConsolidacaoRegistrosPedagogicosDto>(query, new { ueId, anoLetivo });
        }

        public async Task<long> Inserir(ConsolidacaoRegistrosPedagogicos consolidacao)
        {
            return (long)(await database.Conexao.InsertAsync(consolidacao));
        }
    }
}
