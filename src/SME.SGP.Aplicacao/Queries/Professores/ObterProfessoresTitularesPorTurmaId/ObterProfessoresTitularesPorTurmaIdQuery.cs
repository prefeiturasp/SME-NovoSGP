using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
   public class ObterProfessoresTitularesPorTurmaIdQuery : IRequest<IEnumerable<ProfessorTitularDisciplinaEol>>
    {
        public ObterProfessoresTitularesPorTurmaIdQuery(long turmaId, string professorRf = null)
        {
            TurmaId = turmaId;
            ProfessorRf = professorRf;
        }

        public long TurmaId { get; set; }

        public string ProfessorRf { get; set; }
    }
}
