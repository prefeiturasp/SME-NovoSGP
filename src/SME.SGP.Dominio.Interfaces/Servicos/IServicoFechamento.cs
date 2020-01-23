using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoFechamento
    {
        Task<FechamentoDto> ObterPorTipoCalendarioDreEUe(long tipoCalendarioId, long? dreId, long? ueId);
    }
}