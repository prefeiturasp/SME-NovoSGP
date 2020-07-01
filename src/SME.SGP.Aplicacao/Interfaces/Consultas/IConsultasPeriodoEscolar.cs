using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasPeriodoEscolar
    {
        Task<PeriodoEscolar> ObterPeriodoEscolarPorData(long tipoCalendarioId, DateTime dataPeriodo);
        Task<PeriodoEscolarListaDto> ObterPorTipoCalendario(long tipoCalendarioId);
        Task<DateTime> ObterFimPeriodoRecorrencia(long tipoCalendarioId, DateTime inicioRecorrencia, RecorrenciaAula recorrencia);
        Task<int> ObterBimestre(DateTime data, Modalidade modalidade, int semestre = 0);
        Task<IEnumerable<PeriodoEscolar>> ObterPeriodosEmAberto(long ueId, Modalidade modalidadeCodigo, int anoLetivo);
        Task<PeriodoEscolar> ObterUltimoPeriodoAsync(int anoLetivo, ModalidadeTipoCalendario modalidade, int semestre);
        Task<PeriodoEscolar> ObterPeriodoPorModalidade(Modalidade modalidade, DateTime data, int semestre = 0);
        Task<PeriodoEscolar> ObterPeriodoAtualPorModalidade(Modalidade modalidade);
        PeriodoEscolar ObterPeriodoPorData(IEnumerable<PeriodoEscolar> periodosEscolares, DateTime data);
        PeriodoEscolar ObterUltimoPeriodoPorData(IEnumerable<PeriodoEscolar> periodosEscolares, DateTime data);
        Task<IEnumerable<PeriodoEscolar>> ObterPeriodosEscolares(long tipoCalendarioId);
        Task<PeriodoEscolar> ObterUltimoPeriodoAbertoAsync(Turma turma);
        Task<PeriodoEscolar> ObterPeriodoEscolarEmAberto(Modalidade modalidadeCodigo, int anoLetivo);
    }
}