using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasPlanoAnual
    {
        Task<PlanoAnualCompletoDto> ObterBimestreExpandido(FiltroPlanoAnualBimestreExpandidoDto filtro);

        long ObterIdPlanoAnualPorAnoEscolaBimestreETurma(int ano, string escolaId, long turmaId, int bimestre, long disciplinaId);
        Task<PlanoAnualResumoDto> ObterPlanoAnualPorAnoEscolaBimestreETurma(int ano, string escolaId, long turmaId, int bimestre, long disciplinaId);

        Task<PlanoAnualObjetivosDisciplinaDto> ObterObjetivosEscolaTurmaDisciplina(FiltroPlanoAnualDisciplinaDto filtro);

        Task<PlanoAnualCompletoDto> ObterPorEscolaTurmaAnoEBimestre(FiltroPlanoAnualDto filtroPlanoAnualDto, bool seNaoExistirRetornaNovo = true);

        Task<IEnumerable<PlanoAnualCompletoDto>> ObterPorUETurmaAnoEComponenteCurricular(string ueId, string turmaId, int anoLetivo, long componenteCurricularEolId);

        Task<IEnumerable<TurmaParaCopiaPlanoAnualDto>> ObterTurmasParaCopia(int turmaId, long componenteCurricular);

        bool ValidarPlanoAnualExistente(FiltroPlanoAnualDto filtroPlanoAnualDto);
    }
}