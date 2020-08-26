using System;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoDiaLetivo
    {
        Task<bool> ValidarSeEhDiaLetivo(DateTime data, long tipoCalendarioId, string dreId, string ueId);

        Task<bool> ValidarSeEhDiaLetivo(DateTime dataInicio, DateTime dataFim, long tipoCalendarioId, bool ehLetivo = false, long tipoEventoId = 0);

        bool ValidaSeEhLiberacaoExcepcional(DateTime data, long tipoCalendarioId, string ueId);
    }
}