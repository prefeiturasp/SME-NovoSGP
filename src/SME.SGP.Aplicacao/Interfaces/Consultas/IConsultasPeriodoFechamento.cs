using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasPeriodoFechamento
    {
        Task<FechamentoDto> ObterPorTipoCalendarioSme(FiltroFechamentoDto fechamentoDto);
        Task<IEnumerable<PeriodoEscolar>> ObterPeriodosComFechamentoEmAberto(long ueId, int anoLetivo);
        Task<bool> TurmaEmPeriodoDeFechamento(Turma turma, TipoCalendario tipoCalendario, DateTime dataReferencia, int bimestre = 0);
        Task<bool> TurmaEmPeriodoDeFechamento(Turma turma, DateTime dataReferencia, int bimestre = 0);
        Task<PeriodoFechamentoVigenteDto> TurmaEmPeriodoDeFechamentoAnoAnterior(Turma turma, int bimestre = 0);
        Task<PeriodoFechamentoVigenteDto> TurmaEmPeriodoDeFechamentoVigente(Turma turma, DateTime dataReferencia, int bimestre = 0);
    }
}