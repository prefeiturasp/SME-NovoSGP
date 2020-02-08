using System;
using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioConceito : IRepositorioBase<Conceito>
    {
        IEnumerable<Conceito> ObterPorData(DateTime dataAvaliacao);
    }
}