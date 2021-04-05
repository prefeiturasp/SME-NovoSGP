using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAulaPrevistaBimestre : IRepositorioBase<AulaPrevistaBimestre>
    {
        Task<IEnumerable<AulaPrevistaBimestreQuantidade>> ObterBimestresAulasPrevistasPorId(long? aulaPrevistaId);

        Task<IEnumerable<AulaPrevistaBimestreQuantidade>> ObterBimestresAulasPrevistasPorFiltro(long tipoCalendarioId, string turmaId, string disciplinaId);

        Task<IEnumerable<AulaPrevistaBimestre>> ObterAulasPrevistasPorTurmaTipoCalendarioDisciplina(long tipoCalendarioId, string turmaId, string disciplinaId, int? bimestre);
        Task<IEnumerable<AulaPrevistaBimestre>> ObterAulasPrevistasPorTurmaTipoCalendarioBimestre(long tipoCalendarioId, string codigoTurma, int bimestre);
        Task<IEnumerable<AulaPrevistaTurmaComponenteDto>> ObterBimestresAulasTurmasComponentesCumpridasAsync(string[] turmasCodigos, string[] componentesCurricularesId, long tipoCalendarioId, int[] bimestres);
    }
}
