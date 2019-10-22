using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IConsultasPeriodoEscolar
    {
        PeriodoEscolarListaDto ObterPorTipoCalendario(long tipoCalendarioId);
    }
}