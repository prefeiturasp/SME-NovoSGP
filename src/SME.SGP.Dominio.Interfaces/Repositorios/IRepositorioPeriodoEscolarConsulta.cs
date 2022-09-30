﻿using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPeriodoEscolarConsulta : IRepositorioBase<PeriodoEscolar>
    {
        Task<IEnumerable<PeriodoEscolar>> ObterPorTipoCalendario(long tipoCalendarioId);

        Task<IEnumerable<PeriodoEscolar>> ObterPorTipoCalendarioAsync(long tipoCalendarioId);

        Task<PeriodoEscolar> ObterPorTipoCalendarioData(long tipoCalendarioId, DateTime data);

        Task<PeriodoEscolar> ObterPorTipoCalendarioData(long tipoCalendarioId, DateTime dataInicio, DateTime dataFim);

        Task<IEnumerable<PeriodoEscolar>> ObterPeriodosEmAbertoPorTipoCalendarioData(long tipoCalendarioId, DateTime data);

        Task<bool> PeriodoEmAbertoAsync(long tipoCalendarioId, DateTime dataReferencia, int bimestre = 0, bool ehAnoLetivo = false, bool ehModalidadeInfantil = false);
        Task<int> ObterBimestreAtualComAberturaPorTurmaAsync(int anoLetivo, ModalidadeTipoCalendario modalidadeTipoCalendario, long ueId, DateTime dataReferencia);
        Task<IEnumerable<int>> ObterBimestresPorTipoCalendario(long tipoCalendarioId);
        Task<PeriodoEscolar> ObterUltimoBimestreAsync(int anoLetivo, ModalidadeTipoCalendario modalidade, int semestre = 0);
        Task<IEnumerable<PeriodoEscolarModalidadeDto>> ObterPeriodosPassadosNoAno(DateTime data);
        Task<IEnumerable<PeriodoEscolar>> ObterPorModalidadeDataFechamento(int modalidade, DateTime dataFechamento);
        Task<PeriodoEscolar> ObterPorModalidadeAnoEDataFinal(ModalidadeTipoCalendario modalidade, int ano, DateTime dataFim);
        Task<int> ObterBimestreAtualAsync(string codigoTurma, ModalidadeTipoCalendario modalidade, DateTime dataReferencia);
        Task<PeriodoEscolar> ObterPeriodoEscolarAtualPorTurmaIdAsync(string codigoTurma, ModalidadeTipoCalendario modalidade, DateTime dataReferencia);
        Task<PeriodoEscolar> ObterPeriodoEscolarAtualAsync(ModalidadeTipoCalendario modalidadeTipoCalendario, DateTime dataReferencia);
        Task<long> ObterPeriodoEscolarIdPorTurmaBimestre(string turmaCodigo, ModalidadeTipoCalendario modalidadeTipoCalendario, int bimestre, int anoLetivo, int semestre);
        Task<PeriodoEscolar> ObterPeriodoEscolarPorTurmaBimestre(string turmaCodigo, ModalidadeTipoCalendario modalidadeTipoCalendario, int bimestre);
        Task<PeriodoEscolarBimestreDto> ObterPeriodoEscolarPorTurmaBimestreAulaCj(string turmaCodigo, ModalidadeTipoCalendario modalidadeTipoCalendario, int bimestre, bool aulCj);

        Task<long> ObterPeriodoEscolarIdPorTurmaId(long turmaId, ModalidadeTipoCalendario modalidadeTipoCalendario, DateTime dataReferencia);
        Task<long> ObterPeriodoEscolarIdPorTurma(string codigoTurma, ModalidadeTipoCalendario modalidade, DateTime dataReferencia);
        Task<int> ObterBimestreAtualPorTurmaIdAsync(long turmaId, ModalidadeTipoCalendario modalidade, DateTime dataReferencia);
        Task<IEnumerable<PeriodoEscolar>> ObterPorAnoLetivoEModalidadeTurma(int anoLetivo, ModalidadeTipoCalendario modalidadeTipoCalendario, int semestre = 1);
        Task<PeriodoEscolar> ObterPorTipoCalendarioEBimestreAsync(long tipoCalendarioId, int bimestre);
        Task<PeriodoEscolar> ObterUltimoPeriodoEscolarPorData(int anoLetivo, ModalidadeTipoCalendario modalidade, DateTime dataAtual);
        Task<PeriodoEscolar> ObterPeriodoEscolaresPorTurmaBimestre(string turmaCodigo, ModalidadeTipoCalendario modalidadeTipoCalendario, int[] bimestres);
        Task<int> ObterBimestrePorDataPendenciaEModalidade(DateTime dataPendenciaCriada, int modalidadeTipoCalendario);
        Task<IEnumerable<PeriodoEscolarVerificaRegenciaDto>> ObterPeriodoEscolaresPorTurmaComponenteBimestre(string turmaCodigo, long componenteCurricularId, int bimestre, bool aulaCj);
        Task<int> ObterBimestre(long periodoEscolarId);
        Task<int> ObterBimestreAtualComAberturaPorAnoModalidade(int anoLetivo, ModalidadeTipoCalendario modalidadeTipoCalendario, DateTime dataReferencia);
    }
}
