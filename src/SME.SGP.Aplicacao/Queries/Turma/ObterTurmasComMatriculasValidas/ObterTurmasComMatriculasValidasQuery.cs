using System;
using System.Collections.Generic;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasComMatriculasValidasQuery : IRequest<IEnumerable<string>>
    {
        public ObterTurmasComMatriculasValidasQuery(string alunoCodigo, string[] turmasCodigos, DateTime periodoInicio,
            DateTime periodoFim)
        {
            AlunoCodigo = alunoCodigo;
            TurmasCodigos = turmasCodigos;
            PeriodoInicio = periodoInicio;
            PeriodoFim = periodoFim;
        }

        public string AlunoCodigo { get; }
        public string[] TurmasCodigos { get; }
        public DateTime PeriodoInicio { get; }
        public DateTime PeriodoFim { get; }
    }
}