using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPlanejamentoAnualComponente : RepositorioBase<PlanejamentoAnualComponente>, IRepositorioPlanejamentoAnualComponente
    {
        public RepositorioPlanejamentoAnualComponente(ISgpContext database) : base(database)
        {
        }

        public async Task<PlanejamentoAnualComponente> ObterPorPlanejamentoAnualPeriodoEscolarId(long componenteCurricularId, long id)
        {
            var sql = @"select * from planejamento_anual_componente where planejamento_anual_periodo_escolar_id = @id and componente_curricular_id = @componenteCurricularId";
            var planejamento = await database.Conexao.QueryFirstOrDefaultAsync<PlanejamentoAnualComponente>(sql, new { id, componenteCurricularId });
            return planejamento;
        }

        public async Task RemoverPorPlanejamentoAnual(long id)
        {
            var sql = @"delete from planejamento_anual_componente where planejamento_anual_id = @id;
                        delete from planejamento_anual_componente where planejamento_anual_id = @id;";
            await database.Conexao.ExecuteAsync(sql, new { id });
        }
    }
}
