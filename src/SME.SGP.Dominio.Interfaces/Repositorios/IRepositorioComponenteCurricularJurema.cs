using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioComponenteCurricularJurema : IRepositorioBase<ComponenteCurricularJurema>
    {
        IEnumerable<ComponenteCurricularJurema> ObterComponentesJuremaPorCodigoEol(long codigoEol);
    }
}