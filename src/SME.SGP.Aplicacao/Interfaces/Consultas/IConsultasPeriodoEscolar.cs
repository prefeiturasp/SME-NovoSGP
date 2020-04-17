using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasPeriodoEscolar
    {
        PeriodoEscolarDto ObterPeriodoEscolarPorData(long tipoCalendarioId, DateTime dataPeriodo);
        PeriodoEscolarListaDto ObterPorTipoCalendario(long tipoCalendarioId);
        DateTime ObterFimPeriodoRecorrencia(long tipoCalendarioId, DateTime inicioRecorrencia, RecorrenciaAula recorrencia);

        int ObterBimestre(DateTime data, Modalidade modalidade);
        Task<IEnumerable<PeriodoEscolarDto>> ObterPeriodosEmAberto(long ueId, Modalidade modalidadeCodigo, int anoLetivo);
        Task<PeriodoEscolarDto> ObterUltimoBimestreAsync(int anoLetivo, ModalidadeTipoCalendario modalidade, int semestre);
    }
}