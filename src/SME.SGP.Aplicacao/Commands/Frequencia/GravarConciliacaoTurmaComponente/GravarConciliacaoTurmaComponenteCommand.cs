using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class GravarConciliacaoTurmaComponenteCommand : IRequest<bool>
    {
        public GravarConciliacaoTurmaComponenteCommand(string turmaId, string disciplinaId, DateTime dataReferencia, string alunos)
        {
            TurmaId = turmaId;
            DisciplinaId = disciplinaId;
            DataReferencia = dataReferencia;
            Alunos = alunos;
        }

        public string TurmaId { get; }
        public string DisciplinaId { get; }
        public DateTime DataReferencia { get; }
        public string Alunos { get; }
    }
}
