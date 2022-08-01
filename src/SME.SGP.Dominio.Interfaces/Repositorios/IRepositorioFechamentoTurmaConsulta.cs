﻿using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFechamentoTurmaConsulta : IRepositorioBase<FechamentoTurma>
    {
        Task<FechamentoTurma> ObterPorTurmaCodigoBimestreAsync(string turmaCodigo, int bimestre = 0);
        Task<FechamentoTurma> ObterPorTurmaPeriodo(long turmaId, long periodoId = 0);
        Task<FechamentoTurma> ObterPorFechamentoTurmaIdAsync(long fechamentoTurmaId);
        Task<FechamentoTurma> ObterCompletoPorIdAsync(long fechamentoTurmaId);
        Task<bool> VerificaExistePorTurmaCCPeriodoEscolar(long turmaId, long componenteCurricularId, long? periodoEscolarId);
        Task<IEnumerable<FechamentoTurma>> ObterPorTurmaBimestreComponenteCurricular(long turmaId, int bimestre, long componenteCurricularId);
        Task<IEnumerable<FechamentoTurmaDisciplina>> ObterPorTurmaPeriodoCCAsync(long turmaId, long periodoEscolarId, long componenteCurricularId);
        Task<FechamentoTurmaPeriodoEscolarDto> ObterIdEPeriodoPorTurmaBimestre(long turmaId, int? bimestre);
        Task<FechamentoTurma> ObterPorTurma(long turmaId);
    }
}
