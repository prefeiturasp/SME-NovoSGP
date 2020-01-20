using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasFechamento
    {
        FechamentoDto ObterPorTipoCalendarioDreEUe(FiltroFechamentoDto fechamentoDto);
    }
}