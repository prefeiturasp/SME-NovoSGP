using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPeriodoEscolar : IRepositorioBase<PeriodoEscolar>
    {
        IEnumerable<PeriodoEscolar> ObterPorTipoCalendario(long tipoCalendarioId);
    }
}