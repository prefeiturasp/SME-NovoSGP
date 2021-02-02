using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAulaPrevistaBimestre : IRepositorioBase<AulaPrevistaBimestre>
    {
        Task<IEnumerable<AulaPrevistaBimestreQuantidade>> ObterBimestresAulasPrevistasPorId(long? aulaPrevistaId);

        Task<IEnumerable<AulaPrevistaBimestreQuantidade>> ObterBimestresAulasPrevistasPorFiltro(long tipoCalendarioId, string turmaId, string disciplinaId);

        Task<IEnumerable<AulaPrevistaBimestre>> ObterAulasPrevistasPorTurmaTipoCalendarioDisciplina(long tipoCalendarioId, string turmaId, string disciplinaId, int? bimestre);
    }
}
