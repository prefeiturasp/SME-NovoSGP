using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRegistroAusenciaAluno : IRepositorioBase<RegistroAusenciaAluno>
    {
        bool MarcarRegistrosAusenciaAlunoComoExcluidoPorRegistroFrequenciaId(long registroFrequenciaId);

        int ObterTotalAulasPorDisciplinaETurma(DateTime dataAula, string disciplinaId, string turmaId);

        AusenciaPorDisciplinaDto ObterTotalAusenciasPorAlunoETurma(DateTime dataAula, string codigoAluno, string disciplinaId, string turmaId);

        IEnumerable<RegistroAusenciaAluno> ObterRegistrosAusenciaPorAula(long aulaId);
    }
}