using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAulaPrevista : IRepositorioBase<AulaPrevista>
    {
        Task<AulaPrevista> ObterAulaPrevistaFiltro(long tipoCalendarioId, string turmaId, string disciplinaId);

        string ObterProfessorTurmaDisciplinaAulasPrevistasDivergente(int bimestre, string turmaId, string disciplinaId, int limiteDias);
    }
}
