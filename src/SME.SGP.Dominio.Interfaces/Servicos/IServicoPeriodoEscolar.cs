using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoPeriodoEscolar
    {
        Task SalvarPeriodoEscolar(IEnumerable<PeriodoEscolar> periodos, long tipoCalendario);
    }
}