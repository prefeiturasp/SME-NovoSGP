using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPlanejamentoAnualPeriodoEscolar : RepositorioBase<PlanejamentoAnualPeriodoEscolar>, IRepositorioPlanejamentoAnualPeriodoEscolar
    {
        public RepositorioPlanejamentoAnualPeriodoEscolar(ISgpContext database) : base(database)
        {
        }

        public async Task<PlanejamentoAnualPeriodoEscolar> ObterPorPlanejamentoAnualId(long id, long periodoEscolarId)
        {
            var sql = "select * from planejamento_anual_periodo_escolar where planejamento_anual_id = @id and periodo_escolar_id = @periodoEscolarId";
            return await database.Conexao.QueryFirstOrDefaultAsync<PlanejamentoAnualPeriodoEscolar>(sql, new { id, periodoEscolarId });
        }
    }
}
