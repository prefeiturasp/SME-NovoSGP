using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPlanejamentoAnual : IRepositorioBase<PlanejamentoAnual>
    {
        Task<PlanejamentoAnual> ObterPorTurmaEComponenteCurricularPeriodoEscolar(long turmaId, long componenteCurricularId, long periodoEscolarId);
        Task<long> ObterIdPorTurmaEComponenteCurricular(long turmaId, long componenteCurricularId);
        Task<PlanejamentoAnual> ObterPlanejamentoSimplificadoPorTurmaEComponenteCurricular(long turmaId, long componenteCurricularId);
        Task<IEnumerable<TurmaParaCopiaPlanoAnualDto>> ValidaSeTurmasPossuemPlanejamentoAnual(string[] turmasId);
        Task<IEnumerable<TurmaParaCopiaPlanoAnualDto>> ValidaSeTurmasPossuemPlanoAnual(string[] turmasId, bool consideraHistorico);
        Task<IEnumerable<TurmaParaCopiaPlanoAnualDto>> ObterTurmasParaCopiaPlanejamentoAnual(Turma turma, string ano, long componenteCurricularId, string rf, bool ensinoEspecial, bool ehProfessor);

        Task<IEnumerable<TurmaParaCopiaPlanoAnualDto>> ObterTurmasParaCopiaPlanejamentoAnualCP(Turma turma, string ano, bool ensinoEspecial, bool consideraHistorico);


        Task<PlanejamentoAnual> ObterPlanejamentoAnualPorAnoEscolaBimestreETurma(long turmaId, long periodoEscolarId, long componenteCurricularId);
        Task<PlanejamentoAnualDto> ObterPlanejamentoAnualSimplificadoPorTurma(long turmaId);
        Task<long> ExistePlanejamentoAnualParaTurmaPeriodoEComponente(long turmaId, long periodoEscolarId, long componenteCurricularId);
        Task RemoverLogicamenteAsync(long planejamentoAnual);
    }
}
