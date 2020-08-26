using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoCalculoFrequencia
    {
        Task CalcularFrequenciaPorTurma(IEnumerable<string> alunos, DateTime dataAula, string turmaId, string disciplinaId);
    }
}