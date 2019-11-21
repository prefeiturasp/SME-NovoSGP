using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoFrequencia
    {
        IEnumerable<RegistroAusenciaAluno> ObterListaAusenciasPorAula(long aulaId);

        Task Registrar(long aulaId, IEnumerable<RegistroAusenciaAluno> registroAusenciaAlunos);
    }
}