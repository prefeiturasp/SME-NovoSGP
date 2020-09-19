using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
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

        public async Task<PlanejamentoAnualDto> ObterPorTurmaEComponenteCurricular(long turmaId, long componenteCurricularId)
        {
            var sql = @"select
	                        pa.*,
	                        pape.*,
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
                        where
	                        turma_id = @turmaId
	                        and pa.componente_curricular_id = @componenteCurricularId";

            var planejamentos = new Dictionary<long, PlanejamentoAnual>();
            await database.Conexao.QueryAsync<PlanejamentoAnual, PlanejamentoAnualPeriodoEscolar, PlanejamentoAnualComponente, PlanejamentoAnualObjetivoAprendizagem, PlanejamentoAnual>(sql,
                (planejamento, periodo, componente, objetivo) =>
                {

                    return planejamento;
                },
                new { turmaId, componenteCurricularId });
        }
    }
}
