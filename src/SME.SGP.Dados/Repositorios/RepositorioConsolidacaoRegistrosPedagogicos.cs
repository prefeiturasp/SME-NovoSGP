﻿using Dapper;
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

        public async Task<IEnumerable<ConsolidacaoRegistrosPedagogicosDto>> GerarRegistrosPedagogicosComSeparacaoDiarioBordo(string turmaCodigo, int anoLetivo, long[] componentesCurricularesIds)
        {
            const string query = @"with aulas as (
                                    select pe.id as PeriodoEscolarId,
                                        pe.bimestre as Bimestre,
                                        t.id as TurmaId,
                                        t.turma_id as TurmaCodigo,
                                        t.ano_letivo as AnoLetivo,
                                        cast(a.disciplina_id as int8) as DisciplinaId,
                                        a.id as AulaId,
                                        rf.id as RegistroFrequenciaId,
                                        rf.criado_em as RegistroFrequenciaCriadoEm,
                                        pa.id as PlanoAulaId,
                                        pa.criado_em as PlanoAulaCriadoEm,
                                        a.professor_rf as RFProfessor,
                                        t.modalidade_codigo as ModalidadeCodigo,
                                        a.*
                                    from aula a
                                        join periodo_escolar pe on (pe.tipo_calendario_id = a.tipo_calendario_id)
                                        join turma t on (t.turma_id = a.turma_id)
                                        left join registro_frequencia rf on (rf.aula_id = a.id
                                                                        and  not rf.excluido)
                                        left join plano_aula pa on (pa.aula_id = a.id
                                                                and  not pa.excluido)

                                    where not a.excluido
                                    and t.turma_id = @turmaCodigo
                                    and t.ano_letivo = @anoLetivo
                                ),
                                componentePai as (
                                    select a.DisciplinaId 
                                    from aulas a
                                    limit 1
                                ),
                                componentesCurricularesInfantis as (
                                    select cc.id as ComponenteCurricularId,
                                        cc.descricao_infantil as DescricaoInfantil
                                    from componente_curricular cc 
                                        join componentePai cp on (cp.DisciplinaId = cc.componente_curricular_pai_id)
                                    where cc.descricao_infantil is not null
                                ),
                                componentesCurricularesInfantisAulas as (
                                    select distinct cc.*,
                                        a.*
                                    from componentesCurricularesInfantis cc
                                        left join lateral (select * from aulas) a on true
                                   where cc.componentecurricularid = Any(@componentesCurricularesIds)
                                )
                                select distinct cc.PeriodoEscolarId,
                                    cc.Bimestre,
                                    cc.TurmaId,
                                    cc.TurmaCodigo,
                                    cc.AnoLetivo,
                                    cc.ComponenteCurricularId,
                                    count(cc.AulaId) as QuantidadeAulas,
                                    count(cc.AulaId) filter (where cc.RegistroFrequenciaId is null) as FrequenciasPendentes,
                                    max(cc.RegistroFrequenciaCriadoEm) as DataUltimaFrequencia,
                                    max(cc.PlanoAulaCriadoEm) as DataUltimoPlanoAula,
                                    max(db.criado_em) as DataUltimoDiarioBordo,
                                    count(cc.ComponenteCurricularId) filter (where db.id is null) as DiarioBordoPendentes,
                                    count(cc.AulaId) filter (where cc.PlanoAulaId is null) as PlanoAulaPendentes,
                                    null as RFProfessor,
                                    cc.ModalidadeCodigo
                                from componentesCurricularesInfantisAulas cc
                                    left join diario_bordo db on (db.aula_id = cc.AulaId
                                                                and  not db.excluido)
                                group by cc.PeriodoEscolarId, cc.Bimestre, cc.TurmaId, cc.TurmaCodigo, cc.AnoLetivo, 
                                    cc.ComponenteCurricularId, cc.DisciplinaId, cc.RFProfessor, cc.ModalidadeCodigo                                
                                
                                union all

                                select a.PeriodoEscolarId,
                                    a.Bimestre,
                                    a.TurmaId,
                                    a.TurmaCodigo,
                                    a.AnoLetivo,
                                    a.DisciplinaId,
                                    count(a.AulaId) as QuantidadeAulas,
                                    count(a.AulaId) filter (where a.RegistroFrequenciaId is null) as FrequenciasPendentes,
                                    max(a.RegistroFrequenciaCriadoEm) as DataUltimaFrequencia,
                                    max(a.PlanoAulaCriadoEm) as DataUltimoPlanoAula,
                                    null::timestamp as DataUltimoDiarioBordo,
                                    0 as DiarioBordoPendentes,
                                    count(a.AulaId) filter (where a.PlanoAulaId is null) as PlanoAulaPendentes,
                                    a.RFProfessor,
                                    a.ModalidadeCodigo 
                                from aulas a
                                where a.ModalidadeCodigo <> 1
                                group by a.PeriodoEscolarId, a.Bimestre, a.TurmaId, a.TurmaCodigo, a.AnoLetivo,
                                    a.DisciplinaId, a.RFProfessor, a.ModalidadeCodigo";

            return await database.Conexao.QueryAsync<ConsolidacaoRegistrosPedagogicosDto>(query, new { turmaCodigo, anoLetivo, componentesCurricularesIds });
        }
    }
}
