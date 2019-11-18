using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFrequencia : IRepositorioBase<RegistroFrequencia>
    {
        IEnumerable<RegistroAusenciaAluno> ObterListaFrequenciaPorAula(long aulaId);
    }
}