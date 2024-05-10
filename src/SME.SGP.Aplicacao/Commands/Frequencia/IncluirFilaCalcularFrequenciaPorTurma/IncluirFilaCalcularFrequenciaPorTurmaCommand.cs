using MediatR;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class IncluirFilaCalcularFrequenciaPorTurmaCommand : IRequest<bool>
    {
        public IEnumerable<string> Alunos { get; set; }
        public DateTime DataAula { get; set; }
        public string TurmaId { get; set; }
        public string DisciplinaId { get; set; }
        public int[] Meses { get; set; }


        public IncluirFilaCalcularFrequenciaPorTurmaCommand(IEnumerable<string> alunos, DateTime dataAula, string turmaId, string disciplinaId, int[] meses = null)
        {
            Alunos = alunos;
            DataAula = dataAula;
            TurmaId = turmaId;
            DisciplinaId = disciplinaId;
            Meses = meses;
        }
    }
}