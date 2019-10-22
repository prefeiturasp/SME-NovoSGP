using SME.SGP.Dto;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IConsultasPeriodoEscolar
    {
        PeriodoEscolarListaDto ObterPorTipoCalendario(long tipoCalendarioId);
    }
}