using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRegistroAusenciaAlunoConsulta
    {
        Task<int> ObterTotalAulasPorDisciplinaETurma(DateTime dataAula, string disciplinaId, string turmaId);
        AusenciaPorDisciplinaDto ObterTotalAusenciasPorAlunoETurma(DateTime dataAula, string codigoAluno, string disciplinaId, string turmaId);
        Task<AusenciaPorDisciplinaDto> ObterTotalAusenciasPorAlunoETurmaAsync(DateTime dataAula, string codigoAluno, string disciplinaId, string turmaId);
        Task<IEnumerable<RegistroAusenciaAluno>> ObterRegistrosAusenciaPorAulaAsync(long aulaId);
    }
}
