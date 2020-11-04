﻿using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPeriodoEscolar : IRepositorioBase<PeriodoEscolar>
    {
        Task<IEnumerable<PeriodoEscolar>> ObterPorTipoCalendario(long tipoCalendarioId);

        Task<IEnumerable<PeriodoEscolar>> ObterPorTipoCalendarioAsync(long tipoCalendarioId);

        Task<PeriodoEscolar> ObterPorTipoCalendarioData(long tipoCalendarioId, DateTime data);

        Task<PeriodoEscolar> ObterPorTipoCalendarioData(long tipoCalendarioId, DateTime dataInicio, DateTime dataFim);

        Task<IEnumerable<PeriodoEscolar>> ObterPeriodosEmAbertoPorTipoCalendarioData(long tipoCalendarioId, DateTime data);

        Task<bool> PeriodoEmAbertoAsync(long tipoCalendarioId, DateTime dataReferencia, int bimestre = 0, bool ehAnoLetivo = false);

        Task<PeriodoEscolar> ObterUltimoBimestreAsync(int anoLetivo, ModalidadeTipoCalendario modalidade, int semestre = 0);

        Task<int> ObterBimestreAtualAsync(string codigoTurma, ModalidadeTipoCalendario modalidade, DateTime dataReferencia);
        Task<int> ObterBimestreAtualPorTurmaIdAsync(long turmaId, ModalidadeTipoCalendario modalidade, DateTime dataReferencia);
        Task<IEnumerable<PeriodoEscolar>> ObterPorAnoLetivoEModalidadeTurma(int anoLetivo, ModalidadeTipoCalendario modalidadeTipoCalendario, int semestre = 1);
        Task<PeriodoEscolar> ObterPorTipoCalendarioEBimestreAsync(long tipoCalendarioId, int bimestre);
    }
}