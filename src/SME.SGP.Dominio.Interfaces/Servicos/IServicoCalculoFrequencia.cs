using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoCalculoFrequencia
    {
        void CalcularFrequenciaPorTurmaEDisciplina(IEnumerable<string> alunos, long aulaId);

        void CalcularPercentualFrequenciaAlunosPorDisciplinaEPeriodo(int anoLetivo);
    }
}