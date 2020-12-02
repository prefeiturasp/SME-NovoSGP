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
            var sql = @"select * from planejamento_anual_componente where planejamento_anual_periodo_escolar_id = @id and componente_curricular_id = @componenteCurricularId and excluido = false";
            var planejamento = await database.Conexao.QueryFirstOrDefaultAsync<PlanejamentoAnualComponente>(sql, new { id, componenteCurricularId });
            return planejamento;
        }

        public async Task RemoverLogicamenteAsync(long id)
        {
            var sql = "UPDATE planejamento_anual_componente SET EXCLUIDO = TRUE WHERE ID = @id";
            await database.Conexao.ExecuteAsync(sql, new { id });
        }

    }
}
