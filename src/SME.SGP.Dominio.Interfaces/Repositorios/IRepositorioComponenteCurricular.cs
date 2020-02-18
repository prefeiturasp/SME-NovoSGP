using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioComponenteCurricular : IRepositorioBase<ComponenteCurricular>
    {
        IEnumerable<ComponenteCurricular> ObterComponentesJuremaPorCodigoEol(long codigoEol);
    }
}