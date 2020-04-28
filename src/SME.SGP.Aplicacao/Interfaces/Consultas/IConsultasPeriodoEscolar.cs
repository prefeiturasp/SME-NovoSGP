using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasPeriodoEscolar
    {
        PeriodoEscolar ObterPeriodoEscolarPorData(long tipoCalendarioId, DateTime dataPeriodo);
        PeriodoEscolarListaDto ObterPorTipoCalendario(long tipoCalendarioId);
        DateTime ObterFimPeriodoRecorrencia(long tipoCalendarioId, DateTime inicioRecorrencia, RecorrenciaAula recorrencia);

        int ObterBimestre(DateTime data, Modalidade modalidade, int semestre = 0);
        Task<IEnumerable<PeriodoEscolar>> ObterPeriodosEmAberto(long ueId, Modalidade modalidadeCodigo, int anoLetivo);
        Task<PeriodoEscolar> ObterUltimoPeriodoAsync(int anoLetivo, ModalidadeTipoCalendario modalidade, int semestre);
        PeriodoEscolar ObterPeriodoPorModalidade(Modalidade modalidade, DateTime data, int semestre = 0);
        PeriodoEscolar ObterPeriodoAtualPorModalidade(Modalidade modalidade);
        PeriodoEscolar ObterPeriodoPorData(IEnumerable<PeriodoEscolar> periodosEscolares, DateTime data);
        PeriodoEscolar ObterUltimoPeriodoPorData(IEnumerable<PeriodoEscolar> periodosEscolares, DateTime data);
        IEnumerable<PeriodoEscolar> ObterPeriodosEscolares(long tipoCalendarioId);
        Task<PeriodoEscolar> ObterUltimoPeriodoAbertoAsync(Turma turma);
    }
}