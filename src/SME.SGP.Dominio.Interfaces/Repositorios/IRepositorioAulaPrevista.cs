using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAulaPrevista : IRepositorioBase<AulaPrevista>
    {
        string ObterProfessorTurmaDisciplinaAulasPrevistasDivergente(int bimestre, string turmaId, string disciplinaId, int limiteDias);
    }
}
