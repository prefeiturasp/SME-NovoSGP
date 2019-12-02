using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFrequencia : IRepositorioBase<RegistroFrequencia>
    {
        IEnumerable<RegistroAusenciaAluno> ObterListaFrequenciaPorAula(long aulaId);

        RegistroFrequencia ObterRegistroFrequenciaPorAulaId(long aulaId);

        Task ExcluirFrequenciaAula(long aulaId);
    }
}