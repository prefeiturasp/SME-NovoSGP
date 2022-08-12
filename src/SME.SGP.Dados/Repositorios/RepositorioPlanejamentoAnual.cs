using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPlanejamentoAnual : RepositorioBase<PlanejamentoAnual>, IRepositorioPlanejamentoAnual
    {
        public RepositorioPlanejamentoAnual(ISgpContext database) : base(database)
        {
        }

        public override async Task<PlanejamentoAnual> ObterPorIdAsync(long id)
        {
            var sql = @"";
            return await database.Conexao.QueryFirstOrDefaultAsync<PlanejamentoAnual>(sql, new { id });
        }

        public async Task<PlanejamentoAnualPeriodoEscolar> ObterPlanejamentoAnualPeriodoEscolarPorIdAsync(long id)
        {
            var sql = @"";
            return await database.Conexao.QueryFirstOrDefaultAsync<PlanejamentoAnualPeriodoEscolar>(sql, new { id });
        }

        public async Task<PlanejamentoAnual> ObterPorTurmaEComponenteCurricularPeriodoEscolar(long turmaId, long componenteCurricularId, long periodoEscolarId)
        {
            var sql = @"select
	                        pa.*,
	                        pape.*,
                            pe.*,
	                        pac.*,
	                        paoa.*
                        from
	                        planejamento_anual pa
                        inner join planejamento_anual_periodo_escolar pape on
	                        pape.planejamento_anual_id = pa.id
                        inner join planejamento_anual_componente pac on
	                        pac.planejamento_anual_periodo_escolar_id = pape.id
                        inner join planejamento_anual_objetivos_aprendizagem paoa on
	                        paoa.planejamento_anual_componente_id = pac.id
                        inner join periodo_escolar pe on pape.periodo_escolar_id = pe.id
                        where
	                        turma_id = @turmaId
	                        and pa.componente_curricular_id = @componenteCurricularId
	                        and pape.periodo_escolar_id = @periodoEscolarId   
                            and pa.excluido = false
                            and pape.excluido = false
                            and pac.excluido = false
                            and paoa.excluido = false";

            var planejamentos = new List<PlanejamentoAnual>();
            await database.Conexao.QueryAsync<PlanejamentoAnual, PlanejamentoAnualPeriodoEscolar, PeriodoEscolar, PlanejamentoAnualComponente, PlanejamentoAnualObjetivoAprendizagem, PlanejamentoAnual>(sql,
                (planejamento, periodo, periodosEscolares, componente, objetivo) =>
                {
                    PlanejamentoAnual planejamentoAdicionado = planejamentos.FirstOrDefault(c => c.Id == planejamento.Id);
                    if (planejamentoAdicionado == null)
                    {
                        componente.ObjetivosAprendizagem.Add(objetivo);
                        periodo.ComponentesCurriculares.Add(componente);
                        periodo.PeriodoEscolar = periodosEscolares;
                        planejamento.PeriodosEscolares.Add(periodo);
                        planejamentos.Add(planejamento);
                    }
                    else
                    {
                        var periodoEscolar = planejamentoAdicionado.PeriodosEscolares.FirstOrDefault(c => c.Id == periodo.Id);
                        if (periodoEscolar != null)
                        {
                            var componenteCurricular = periodoEscolar.ComponentesCurriculares.FirstOrDefault(c => c.Id == componente.Id);
                            if (componenteCurricular != null)
                            {
                                var objetivoAprendizagem = componenteCurricular.ObjetivosAprendizagem.FirstOrDefault(c => c.Id == objetivo.Id);
                                if (objetivoAprendizagem == null)
                                {
                                    componenteCurricular.ObjetivosAprendizagem.Add(objetivo);
                                }
                            }
                            else
                            {
                                componenteCurricular.ObjetivosAprendizagem.Add(objetivo);
                                periodoEscolar.ComponentesCurriculares.Add(componenteCurricular);
                            }
                        }
                        else
                        {
                            componente.ObjetivosAprendizagem.Add(objetivo);
                            periodo.ComponentesCurriculares.Add(componente);
                            periodo.PeriodoEscolar = periodosEscolares;
                            planejamentoAdicionado.PeriodosEscolares.Add(periodo);
                        }
                    }
                    return planejamento;
                },
                new { turmaId, componenteCurricularId, periodoEscolarId });

            return planejamentos.FirstOrDefault();
        }

        public async Task<PlanejamentoAnual> ObterPlanejamentoSimplificadoPorTurmaEComponenteCurricular(long turmaId, long componenteCurricularId)
        {
            var sql = @"select
	                        pa.*
                        from
	                        planejamento_anual pa
                        where
	                        turma_id = @turmaId
	                        and pa.componente_curricular_id = @componenteCurricularId
                            and pa.excluido = false";

            return await database.Conexao.QueryFirstOrDefaultAsync<PlanejamentoAnual>(sql, new { turmaId, componenteCurricularId });
        }

        public async Task<long> ObterIdPorTurmaEComponenteCurricular(long turmaId, long componenteCurricularId)
        {
            var sql = @"select
	                        pa.id
                        from
	                        planejamento_anual pa
                        where
	                        turma_id = @turmaId
	                        and pa.componente_curricular_id = @componenteCurricularId
                        and pa.excluido = false";

            return await database.Conexao.QueryFirstOrDefaultAsync<long>(sql, new { turmaId, componenteCurricularId });
        }

        public async Task<IEnumerable<TurmaParaCopiaPlanoAnualDto>> ValidaSeTurmasPossuemPlanoAnual(string[] turmasId, bool consideraHistorico)
        {
            var query = $@"select
                            t.*,
                            case when p.turma_id is not null then true else false end as possuiPlano,
                            p.componente_curricular_id as codigoComponenteCurricular,
                            p.bimestre
                            from turma t
                            inner join abrangencia a on a.turma_id = t.id
                            left join (select t.id turma_id, pa.componente_curricular_id, pe.bimestre 
                            from planejamento_anual pa
                            inner join planejamento_anual_periodo_escolar pap on pap.planejamento_anual_id = pa.id and pap.excluido = false
                            inner join planejamento_anual_componente pac on pac.planejamento_anual_periodo_escolar_id = pap.id and pac.excluido = false
                            inner join turma t on pa.turma_id = t.id
                            inner join periodo_escolar pe on pap.periodo_escolar_id = pe.id
                            where pa.excluido = false 
                            and t.turma_id = Any(@turmasId)) p on p.turma_id = t.id
                            where t.turma_id = Any(@turmasId) 
                            and {(consideraHistorico ? string.Empty : "not")} a.historico;";

            return await database.Conexao.QueryAsync<TurmaParaCopiaPlanoAnualDto>(query, new { turmasId });
        }

        public async Task<IEnumerable<TurmaParaCopiaPlanoAnualDto>> ValidaSeTurmasPossuemPlanejamentoAnual(string[] turmasId)
        {
            var query = @"select
	                        t.*,
	                        (select 1 from planejamento_anual where turma_id = t.turma_id::int8 limit 1) as possuiPlano
                        from
	                        turma t
                        inner join abrangencia a on
	                        a.turma_id = t.id
                        left join planejamento_anual p on
	                        p.turma_id = a.turma_id
                        where
	                        t.turma_id = Any(@turmasId) and not a.historico 
                            and p.excluido = false group by t.id";

            return await database.Conexao.QueryAsync<TurmaParaCopiaPlanoAnualDto>(query, new { turmasId });
        }

        public async Task<IEnumerable<TurmaParaCopiaPlanoAnualDto>> ObterTurmasParaCopiaPlanejamentoAnual(Turma turma, string ano, long componenteCurricularId, string rf, bool ensinoEspecial, bool ehProfessor)
        {
            var query = @"select
	                        t.id,
	                        t.id as TurmaId,
	                        t.nome as nome,
	                        (
	                        select
		                        1
	                        from
		                        planejamento_anual
	                        where
		                        turma_id = t.id
	                        limit 1) as possuiPlano
                        from
	                        turma t
                        inner join abrangencia ab on
	                        t.id = ab.turma_id
                        inner join aula a on
	                        a.turma_id = t.turma_id 
                        left join planejamento_anual p on
	                        p.turma_id = ab.turma_id
                        where
	                        not ab.historico and a.disciplina_id = @componenteCurricularId and t.id <> @turmaId and t.ue_id = @ueId and p.excluido = false";
            if (ehProfessor)
                query += " and a.criado_rf = @rf ";
            if (!ensinoEspecial)
                query += "and t.ano = @ano";
            query += $" group by t.id order by t.nome";

            return await database.Conexao.QueryAsync<TurmaParaCopiaPlanoAnualDto>(query, new { turmaId = turma.Id, ueId = turma.UeId, componenteCurricularId = componenteCurricularId.ToString(), ano, rf });
        }

        public async Task<IEnumerable<TurmaParaCopiaPlanoAnualDto>> ObterTurmasParaCopiaPlanejamentoAnualCP(Turma turma, string ano, bool ensinoEspecial, bool consideraHistorico)
        {
            var query = $@"select
	                        t.id,
	                        t.id as TurmaId,
	                        t.nome as nome,
	                        (
	                        select
		                        1
	                        from
		                        planejamento_anual
	                        where
		                        turma_id = t.id and excluido = false
	                        limit 1) as possuiPlano
                        from
	                        turma t
                        inner join abrangencia ab on
	                        t.id = ab.turma_id
                        left join planejamento_anual p on
	                        p.turma_id = ab.turma_id
                        where
	                        {(consideraHistorico ? string.Empty : "not")} ab.historico and t.id <> @turmaId and t.ue_id = @ueId and p.excluido = false
                            {(!ensinoEspecial ? " and t.ano = @ano " : "")}  
                        group by t.id order by t.nome  ";

            return await database.Conexao.QueryAsync<TurmaParaCopiaPlanoAnualDto>(query, new { turmaId = turma.Id, ueId = turma.UeId, ano });
        }

        public async Task<PlanejamentoAnual> ObterPlanejamentoAnualPorAnoEscolaBimestreETurma(long turmaId, long periodoEscolarId, long componenteCurricularId)
        {
            var query = @"select pa.id, pa.turma_id, pa.componente_curricular_id, pa.migrado, 
	                        pa.criado_em, pa.alterado_em, pa.criado_por, pa.alterado_por, pa.criado_rf, pa.alterado_rf
                        from planejamento_anual pa
                        inner join planejamento_anual_periodo_escolar pe on pe.planejamento_anual_id = pa.id
                        where turma_id = @turmaId 
                          and periodo_escolar_id = @periodoEscolarId 
                          and componente_curricular_id = @componenteCurricularId
                          and pa.excluido = false 
                          and pe.excluido = false ";

            return await database.Conexao.QueryFirstOrDefaultAsync<PlanejamentoAnual>(query.ToString(),
                new
                {
                    turmaId,
                    periodoEscolarId,
                    componenteCurricularId
                });

        }

        public async Task<PlanejamentoAnualDto> ObterPlanejamentoAnualSimplificadoPorTurma(long turmaId)
        {
            var sql = @"select
	                        id, 	
	                        turma_id, 	
	                        componente_curricular_id	
                        from
	                        planejamento_anual pa
                        where
	                        turma_id = @turmaId 
                        and pa.excluido = false";

            return await database.Conexao.QueryFirstOrDefaultAsync<PlanejamentoAnualDto>(sql, new { turmaId });
        }

        public async Task<long> ExistePlanejamentoAnualParaTurmaPeriodoEComponente(long turmaId, long periodoEscolarId, long componenteCurricularId)
        {
            var query = @"select pe.id
                            from planejamento_anual pa
                           inner join planejamento_anual_periodo_escolar pe on pe.planejamento_anual_id = pa.id
                           where turma_id = @turmaId 
                             and periodo_escolar_id = @periodoEscolarId 
                             and componente_curricular_id = @componenteCurricularId 
                             and pa.excluido = false 
                             and pe.excluido = false";

            return await database.Conexao.QueryFirstOrDefaultAsync<long>(query.ToString(),
                new
                {
                    turmaId,
                    periodoEscolarId,
                    componenteCurricularId
                });
        }

        public async Task RemoverLogicamenteAsync(long id)
        {
            var sql = @"UPDATE planejamento_anual pa
                        SET excluido = true
                          , alterado_por = @alteradoPor
                          , alterado_rf = @alteradoRF
                          , alterado_em = @alteradoEm 
                        WHERE ID = @id and
                        not exists(select 1
                                    from planejamento_anual_periodo_escolar pape
                                        inner join planejamento_anual_componente pac
                                            on pape.id = pac.planejamento_anual_periodo_escolar_id                                        
                                   where pape.planejamento_anual_id = pa.id and
                                  		 not pape.excluido and
                                  		 not pac.excluido);";
            await database.Conexao.ExecuteAsync(sql, new
            {
                id,
                alteradoPor = database.UsuarioLogadoNomeCompleto,
                alteradoRF = database.UsuarioLogadoRF,
                alteradoEm = DateTimeExtension.HorarioBrasilia()
            });
        }
    }
}
