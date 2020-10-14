using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPlanejamentoAnualPeriodoEscolar : RepositorioBase<PlanejamentoAnualPeriodoEscolar>, IRepositorioPlanejamentoAnualPeriodoEscolar
    {
        public RepositorioPlanejamentoAnualPeriodoEscolar(ISgpContext database) : base(database)
        {
        }

        public async Task<PlanejamentoAnualPeriodoEscolar> ObterPorPlanejamentoAnualIdEPeriodoId(long id, long periodoEscolarId)
        {
            var sql = "select * from planejamento_anual_periodo_escolar where planejamento_anual_id = @id and periodo_escolar_id = @periodoEscolarId";
            return await database.Conexao.QueryFirstOrDefaultAsync<PlanejamentoAnualPeriodoEscolar>(sql, new { id, periodoEscolarId });
        }

        public async Task<PlanejamentoAnualPeriodoEscolar> ObterPlanejamentoAnualPeriodoEscolarPorTurmaEComponenteCurricular(long turmaId, long componenteCurricularId, long periodoEscolarId)
        {
            var sql = @"select
	                        distinct pape.*,
                            pe.*,
	                        pac.*,
	                        paoa.*
                        from
	                        planejamento_anual pa
                        inner join planejamento_anual_periodo_escolar pape on
	                        pape.planejamento_anual_id = pa.id
                        inner join planejamento_anual_componente pac on
	                        pac.planejamento_anual_periodo_escolar_id = pape.id
                        left join planejamento_anual_objetivos_aprendizagem paoa on
	                        paoa.planejamento_anual_componente_id = pac.id
                        inner join periodo_escolar pe on pape.periodo_escolar_id = pe.id
                        where
	                        turma_id = @turmaId
	                        and pac.componente_curricular_id = @componenteCurricularId
	                        and pape.periodo_escolar_id = @periodoEscolarId
                        order by paoa.objetivo_aprendizagem_id";

            var periodos = new List<PlanejamentoAnualPeriodoEscolar>();
            await database.Conexao.QueryAsync<PlanejamentoAnualPeriodoEscolar, PeriodoEscolar, PlanejamentoAnualComponente, PlanejamentoAnualObjetivoAprendizagem, PlanejamentoAnualPeriodoEscolar>(sql,
                (periodo, periodoEscolar, componente, objetivo) =>
                {
                    var periodoAdicionado = periodos.FirstOrDefault(c => c.Id == periodo.Id);
                    if (periodoAdicionado == null)
                    {
                        componente.ObjetivosAprendizagem.Add(objetivo);
                        periodo.ComponentesCurriculares.Add(componente);
                        periodo.PeriodoEscolar = periodoEscolar;
                        periodos.Add(periodo);
                    }
                    else
                    {
                        var componenteCurricular = periodoAdicionado.ComponentesCurriculares.FirstOrDefault(c => c.Id == componente.Id);
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
                            periodoAdicionado.ComponentesCurriculares.Add(componenteCurricular);
                        }
                    }
                    return periodo;
                },
                new { turmaId, componenteCurricularId, periodoEscolarId });

            return periodos.FirstOrDefault();
        }

        public async Task<IEnumerable<PlanejamentoAnualPeriodoEscolar>> ObterCompletoPorIdAsync(long[] ids)
        {
            List<PlanejamentoAnualPeriodoEscolar> retorno = new List<PlanejamentoAnualPeriodoEscolar>();

            var sql = @"select
                            id,
	                        periodo_escolar_id,
	                        planejamento_anual_id
                        from
	                        planejamento_anual_periodo_escolar
                        where
	                        planejamento_anual_periodo_escolar.id = ANY(@ids);

                        select
                            id,
	                        componente_curricular_id,
	                        descricao,
	                        planejamento_anual_periodo_escolar_id
                        from
	                        planejamento_anual_componente
                        where planejamento_anual_periodo_escolar_id = ANY(@ids);

                        select
                            planejamento_anual_objetivos_aprendizagem.id,
	                        planejamento_anual_objetivos_aprendizagem.planejamento_anual_componente_id,
	                        planejamento_anual_objetivos_aprendizagem.objetivo_aprendizagem_id
                        from
	                        planejamento_anual_objetivos_aprendizagem
                        inner join planejamento_anual_componente on
	                        planejamento_anual_objetivos_aprendizagem.planejamento_anual_componente_id = planejamento_anual_componente.id
                        where
	                        planejamento_anual_componente.planejamento_anual_periodo_escolar_id = ANY(@ids);";

            using (var multi = await database.Conexao.QueryMultipleAsync(sql, new { ids }))
            {
                retorno = multi.Read<PlanejamentoAnualPeriodoEscolar>().ToList();
                var componentes = multi.Read<PlanejamentoAnualComponente>().ToList();
                var objetivoAprendizagems = multi.Read<PlanejamentoAnualObjetivoAprendizagem>().ToList();

                componentes.ForEach(c => c.ObjetivosAprendizagem.AddRange(objetivoAprendizagems.Where(oa => oa.PlanejamentoAnualComponenteId == c.Id)));

                retorno.ForEach(pe =>
                    pe.ComponentesCurriculares.AddRange(componentes.Where(c =>
                        c.PlanejamentoAnualPeriodoEscolarId == pe.Id)));
            }

            return retorno;
        }

        public async Task<IEnumerable<PlanejamentoAnualPeriodoEscolarResumoDto>> ObterPorPlanejamentoAnualId(long planejamentoAnualId)
        {
            var sql = @"select
	                    pape.id,
	                    bimestre
                    from
	                    planejamento_anual_periodo_escolar pape
                    inner join periodo_escolar pe on
	                    pape.periodo_escolar_id = pe.id
                    where planejamento_anual_id = @planejamentoAnualId";
            return await database.Conexao.QueryAsync<PlanejamentoAnualPeriodoEscolarResumoDto>(sql, new { planejamentoAnualId });
        }
    }
}
