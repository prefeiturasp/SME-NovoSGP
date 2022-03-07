using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
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
            const string query = @"select 1 
                                     from consolidacao_registros_pedagogicos c
                                          inner join turma t on t.id = c.turma_id
                                    where t.ano_letivo = @ano";

            return await database.Conexao.QueryFirstOrDefaultAsync<bool>(query, new { ano });
        }

        public async Task ExcluirPorAno(int anoLetivo)
        {
            const string query = "delete from consolidacao_registros_pedagogicos where ano_letivo = @anoLetivo";

            await database.Conexao.ExecuteScalarAsync(query, new { anoLetivo });
        }

        public async Task Excluir(ConsolidacaoRegistrosPedagogicos consolidacao)
        {
            const string query = @"delete from consolidacao_registros_pedagogicos 
                                    where turma_id = @turmaId 
                                      and componente_curricular_id = @componenteCurricularId 
                                      and periodo_escolar_id = @periodoEscolarId 
                                      and ano_letivo = @anoLetivo 
                                      and rf_professor = @rfProfessor";

            var parametros = new
            {
                consolidacao.TurmaId,
                consolidacao.ComponenteCurricularId,
                consolidacao.PeriodoEscolarId,
                consolidacao.AnoLetivo,
                consolidacao.RFProfessor
            };

            await database.Conexao.ExecuteScalarAsync(query, parametros);
        }

        public async Task<IEnumerable<ConsolidacaoRegistrosPedagogicosDto>> GerarRegistrosPedagogicos(long ueId, int anoLetivo)
        {
            const string query = @"select 					      
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
                                    inner join periodo_fechamento_bimestre pfb on pfb.periodo_escolar_id = pe.id 
                                                and a.data_aula between pfb.inicio_fechamento and pfb.final_fechamento
                                    left join registro_frequencia rf on rf.aula_id = a.id and not rf.excluido 
                                    left join plano_aula pa on pa.aula_id = a.id and not pa.excluido 
                                where not a.excluido 
                                and t.ue_id = @ueId
                                and t.ano_letivo = @anoLetivo
                                group by pe.id, pe.bimestre, t.id, a.disciplina_id, a.professor_rf, t.modalidade_codigo";

            return await database.Conexao.QueryAsync<ConsolidacaoRegistrosPedagogicosDto>(query, new { ueId, anoLetivo });
        }

        public async Task<long> Inserir(ConsolidacaoRegistrosPedagogicos consolidacao)
        {
            return (long)await database.Conexao.InsertAsync(consolidacao);
        }

        public async Task<IEnumerable<ConsolidacaoRegistrosPedagogicosDto>> GerarRegistrosPedagogicosComSeparacaoDiarioBordo(long ueId, int anoLetivo)
        {
            const string query = @"with aulas as (
                                       select a.id as aulaid,
                                              cast(a.disciplina_id as integer) as componentecurricularpaiid,
                                              a.data_aula as dataaula,
                                              a.professor_rf as rfprofessor,
                                              pe.id as periodoescolarid,
                                              pe.bimestre,
                                              t.id as turmaid,
                                              t.turma_id as turmacodigo,
                                              t.ano_letivo as anoletivo,
                                              t.modalidade_codigo as modalidadecodigo,
                                              rf.id as rfid,
                                              rf.criado_em as rfcriadoem,
                                              pa.criado_em as pacriadoem,
                                              pa.id as planoaulaid,
                                              db.criado_em as dbcriadoem,
                                              db.id as diariobordoid,
                                              db.componente_curricular_id
                                         from aula a
                                              inner join periodo_escolar pe on (pe.tipo_calendario_id = a.tipo_calendario_id)
                                              inner join turma t on (t.turma_id = a.turma_id)
                                              inner join periodo_fechamento_bimestre pfb on (pfb.periodo_escolar_id = pe.id
                                                                                        and  a.data_aula between pfb.inicio_fechamento and pfb.final_fechamento)
                                              left outer join registro_frequencia rf on (rf.aula_id = a.id
                                                                                    and  not rf.excluido)
                                              left outer join plano_aula pa on (pa.aula_id = a.id
                                                                           and not pa.excluido)
                                              left outer join diario_bordo db on (db.aula_id = a.id
                                                                             and not db.excluido)
                                        where not a.excluido
                                          and t.ue_id = @ueId
                                          and t.ano_letivo = @anoLetivo
                                    ),
                                    componentePai as (
                                       select als.componentecurricularpaiid
                                         from aulas als
                                         limit 1
                                    ),
                                    componentesCurriculares as (
                                       select cc.id as componentecurricularid,
                                              cc.descricao_infantil as descricaoinfantil,
                                              als.dataaula,
                                              als.aulaid
                                         from componente_curricular cc
                                              inner join componentePai cp on (cp.componentecurricularpaiid = cc.componente_curricular_pai_id)
                                              inner join aulas als on (als.componente_curricular_id = cc.id)
                                          where cc.descricao_infantil is not null
                                    )
                                    select distinct als.periodoescolarid,
                                           als.bimestre,
                                           als.turmaid,
                                           als.turmacodigo,
                                           als.anoletivo,
                                           als.componentecurricularpaiid,
                                           cc.descricaoinfantil as componentecurricular,
                                           count(als.aulaid) as quantidadeaulas,
                                           count(als.aulaid) filter (where als.rfid is null) as frequenciaspendentes,
                                           max(als.rfcriadoem) as dataultimafrequencia,
                                           max(als.pacriadoem) as dataultimoplanoaula,
                                           max(als.dbcriadoem) as dataultimodiariobordo,
                                           count(als.aulaid) filter (where als.diariobordoid is null) as diariobordopendentes,
                                           count(als.aulaid) filter (where als.planoaulaid is null) as planoaulapendentes,
                                           als.rfprofessor,
                                           als.modalidadecodigo 
                                      from aulas als
                                           inner join componentesCurriculares cc on (cc.aulaid = als.aulaid)
                                           left outer join diario_bordo db on (db.aula_id = cc.aulaid
                                                                          and  db.componente_curricular_id = cc.componentecurricularid)  
                                   group by als.periodoescolarid,
                                            als.bimestre,
                                            als.turmaid,
                                            als.turmacodigo,
                                            als.anoletivo,
                                            als.componentecurricularpaiid,
                                            cc.descricaoinfantil,
                                            als.rfprofessor,
                                            als.modalidadecodigo";

            return await database.Conexao.QueryAsync<ConsolidacaoRegistrosPedagogicosDto>(query, new { ueId, anoLetivo });
        }
    }
}
