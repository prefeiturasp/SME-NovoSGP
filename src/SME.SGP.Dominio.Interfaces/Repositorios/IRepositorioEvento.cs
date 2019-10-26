using System;
using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioEvento : IRepositorioBase<Evento>
    {
        IEnumerable<Evento> ObterEventosPorData(DateTime dataInicio);
    }
}