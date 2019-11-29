using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRegistroAusenciaAluno : IRepositorioBase<RegistroAusenciaAluno>
    {
        bool MarcarRegistrosAusenciaAlunoComoExcluidoPorRegistroFrequenciaId(long registroFrequenciaId);

        IEnumerable<AulasPorDisciplinaDto> ObterTotalAulasPorDisciplina(int anoLetivo);

        int ObterTotalAulasPorDisciplinaETurma(DateTime dataAtual, string disciplinaId, string turmaId);

        AusenciaPorDisciplinaDto ObterTotalAusenciasPorAlunoETurma(DateTime periodo, string codigoAluno, string disciplinaId, string turmaId);
    }
}