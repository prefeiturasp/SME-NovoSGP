using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioConceito : IRepositorioBase<Conceito>
    {
       Task<IEnumerable<Conceito>> ObterPorData(DateTime dataAvaliacao);
    }
}