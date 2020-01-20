using SME.SGP.Infra;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoFechamento
    {
        FechamentoDto ObterPorTipoCalendarioDreEUe(long tipoCalendarioId, string dreId, string ueId);
    }
}