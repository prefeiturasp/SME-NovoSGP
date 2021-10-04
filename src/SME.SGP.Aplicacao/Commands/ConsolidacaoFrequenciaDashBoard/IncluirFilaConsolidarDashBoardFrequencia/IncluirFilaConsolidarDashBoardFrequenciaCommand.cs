using MediatR;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class IncluirFilaConsolidarDashBoardFrequenciaCommand : IRequest<bool>
    {
        public IncluirFilaConsolidarDashBoardFrequenciaCommand(IEnumerable<string> alunos, DateTime dataAula, string turmaId)
        {
            Alunos = alunos;
            DataAula = dataAula;
            TurmaId = turmaId;
        }

        public IEnumerable<string> Alunos { get; set; }
        public DateTime DataAula { get; set; }
        public string TurmaId { get; set; }
    }
}
