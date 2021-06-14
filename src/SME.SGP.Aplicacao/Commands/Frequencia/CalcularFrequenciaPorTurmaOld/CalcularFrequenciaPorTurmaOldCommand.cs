using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class CalcularFrequenciaPorTurmaOldCommand : IRequest<bool>
    {
        public CalcularFrequenciaPorTurmaOldCommand(IEnumerable<string> alunos, DateTime dataAula, string turmaId, string disciplinaId)
        {
            Alunos = alunos;
            DataAula = dataAula;
            TurmaId = turmaId;
            DisciplinaId = disciplinaId;
        }

        public IEnumerable<string> Alunos { get; }
        public DateTime DataAula { get; }
        public string TurmaId { get; }
        public string DisciplinaId { get; }
    }
}
