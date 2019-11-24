using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasPeriodoEscolar
    {
        PeriodoEscolarListaDto ObterPorTipoCalendario(long tipoCalendarioId);
    }
}