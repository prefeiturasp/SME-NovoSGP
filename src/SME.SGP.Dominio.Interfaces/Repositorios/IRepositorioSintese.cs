using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioSintese : IRepositorioBase<Sintese>
    {
        IEnumerable<Sintese> ObterPorData(DateTime dataAvaliacao);
    }
}
