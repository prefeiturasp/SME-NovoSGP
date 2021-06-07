﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFechamentoTurma : IRepositorioBase<FechamentoTurma>
    {
        Task<FechamentoTurma> ObterPorTurmaCodigoBimestreAsync(string turmaCodigo, int bimestre = 0);
        Task<FechamentoTurma> ObterPorTurmaPeriodo(long turmaId, long periodoId = 0);
        Task<FechamentoTurma> ObterCompletoPorIdAsync(long fechamentoTurmaId);
        Task<bool> VerificaExistePorTurmaCCPeriodoEscolar(long turmaId, long componenteCurricularId, long? periodoEscolarId);
        Task<IEnumerable<FechamentoTurmaDisciplina>> ObterPorTurmaPeriodoCCAsync(long turmaId, long periodoEscolarId, long componenteCurricularId);
    }
}
