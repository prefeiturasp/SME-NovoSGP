using System;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRegistroAusenciaAlunoConsulta
    {
        Task<int> ObterTotalAulasPorDisciplinaETurma(DateTime dataAula, string disciplinaId, string turmaId);
    }
}
