using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasPeriodoEscolar
    {
        PeriodoEscolarDto ObterPeriodoEscolarPorData(long tipoCalendarioId, DateTime dataPeriodo);
        PeriodoEscolarListaDto ObterPorTipoCalendario(long tipoCalendarioId);
        DateTime ObterFimPeriodoRecorrencia(long tipoCalendarioId, DateTime inicioRecorrencia, RecorrenciaAula recorrencia);

        int ObterBimestre(DateTime data, Modalidade modalidade);
    }
}