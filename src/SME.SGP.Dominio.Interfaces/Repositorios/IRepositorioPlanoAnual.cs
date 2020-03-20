using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPlanoAnual : IRepositorioBase<PlanoAnual>
    {
        PlanoAnualCompletoDto ObterPlanoAnualCompletoPorAnoEscolaBimestreETurma(int ano, string escolaId, string turmaId, int bimestre, long componenteCurricularEolId);

        IEnumerable<PlanoAnualCompletoDto> ObterPlanoAnualCompletoPorAnoUEETurma(int ano, string ueId, string turmaId, long componenteCurricularEolId);

        Task<PlanoAnual> ObterPlanoAnualSimplificadoPorAnoEscolaBimestreETurma(int ano, string escolaId, long turmaId, int bimestre, long disciplinaId);
        Task<long> ObterPlanoAnualIdPorAnoEscolaBimestreETurma(int ano, string escolaId, long turmaId, int bimestre, long disciplinaId);

        PlanoAnualObjetivosDisciplinaDto ObterPlanoObjetivosEscolaTurmaDisciplina(int ano, string escolaId, string turmaId, int bimestre, long componenteCurricularEolId, long disciplinaId);

        bool ValidarPlanoExistentePorAnoEscolaTurmaEBimestre(int ano, string escolaId, string turmaId, int bimestre, long componenteCurricularEolId);

        IEnumerable<TurmaParaCopiaPlanoAnualDto> ValidaSeTurmasPossuemPlanoAnual(string[] turmasId);
    }
}