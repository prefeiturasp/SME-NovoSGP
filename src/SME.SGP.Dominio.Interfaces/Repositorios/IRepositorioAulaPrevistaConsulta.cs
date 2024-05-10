using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAulaPrevistaConsulta : IRepositorioBase<AulaPrevista>
    {
        Task<AulaPrevista> ObterAulaPrevistaFiltro(long tipoCalendarioId, string turmaId, string disciplinaId);
        Task<IEnumerable<AulaPrevista>> ObterAulasPrevistasPorUe(long codigoUe);
        string ObterProfessorTurmaDisciplinaAulasPrevistasDivergente(int bimestre, string turmaId, string disciplinaId, int limiteDias);
    }
}
