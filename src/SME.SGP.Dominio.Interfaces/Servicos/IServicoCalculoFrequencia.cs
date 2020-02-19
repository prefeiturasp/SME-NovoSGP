using System;
using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoCalculoFrequencia
    {
        void CalcularFrequenciaPorTurma(IEnumerable<string> alunos, DateTime dataAula, string turmaId, string disciplinaId);
    }
}