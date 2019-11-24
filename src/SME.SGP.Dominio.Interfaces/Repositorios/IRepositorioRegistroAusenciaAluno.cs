using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRegistroAusenciaAluno : IRepositorioBase<RegistroAusenciaAluno>
    {
        bool MarcarRegistrosAusenciaAlunoComoExcluidoPorRegistroFrequenciaId(long registroFrequenciaId);

        IEnumerable<AulasPorDisciplinaDto> ObterTotalAulasPorDisciplina(int anoLetivo);

        IEnumerable<AusenciaPorDisciplinaDto> ObterTotalAusenciasPorAlunoEDisciplina(int anoLetivo);
    }
}