using SME.SGP.Infra;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoFechamento
    {
        FechamentoDto ObterPorTipoCalendarioDreEUe(long tipoCalendarioId, long? dreId, long? ueId);
    }
}