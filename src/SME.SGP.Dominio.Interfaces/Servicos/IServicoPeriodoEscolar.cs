using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoPeriodoEscolar
    {
        void SalvarPeriodoEscolar(IEnumerable<PeriodoEscolar> periodos, long tipoCalendario);
    }
}