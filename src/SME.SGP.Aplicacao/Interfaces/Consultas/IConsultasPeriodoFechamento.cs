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
        Task<IEnumerable<PeriodoEscolar>> ObterPeriodosComFechamentoEmAberto(long ueId);
        Task<bool> TurmaEmPeriodoDeFechamento(string turmaCodigo, DateTime dataReferencia, int bimestre = 0);
        Task<bool> TurmaEmPeriodoDeFechamento(Turma turma, TipoCalendario tipoCalendario, DateTime dataReferencia, int bimestre = 0);
        Task<bool> TurmaEmPeriodoDeFechamento(Turma turma, DateTime dataReferencia, int bimestre = 0);
        Task<bool> TurmaEmPeriodoDeFechamentoAula(Turma turma, DateTime dataReferencia, int bimestreAtual = 0, int bimestreAlteracao = 0);
        Task<PeriodoFechamentoVigenteDto> TurmaEmPeriodoDeFechamentoVigente(Turma turma, DateTime dataReferencia, int bimestre = 0);
        Task<PeriodoFechamentoVigenteDto> TurmaEmPeriodoDeFechamentoVigente(string turmaCodigo, DateTime dataReferencia, int bimestre = 0);
    }
}