using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPlanejamentoAnual : IRepositorioBase<PlanejamentoAnual>
    {
        Task<PlanejamentoAnual> ObterPorTurmaEComponenteCurricular(long turmaId, long componenteCurricularId, long periodoEscolarId);
        Task<PlanejamentoAnual> ObterPlanejamentoSimplificadoPorTurmaEComponenteCurricular(long turmaId, long componenteCurricularId);

        Task<PlanejamentoAnual> ObterPlanejamentoAnualPorAnoEscolaBimestreETurma(int ano, string escolaId, long turmaId, int bimestre, long disciplinaId);
    }
}
