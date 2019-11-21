using System;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoDiaLetivo
    {
        bool ValidarSeEhDiaLetivo(DateTime data, long tipoCalendarioId, string dreId, string ueId);

        bool ValidarSeEhDiaLetivo(DateTime dataInicio, DateTime dataFim, long tipoCalendarioId, bool ehLetivo = false, long TipoEventoId = 0);
    }
}