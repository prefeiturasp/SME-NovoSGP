using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterProfessoresTitularesDaTurmaCompletosQuery : IRequest<IEnumerable<ProfessorTitularDisciplinaEol>>
    {
        public ObterProfessoresTitularesDaTurmaCompletosQuery(string codigoTurma)
        {
            CodigoTurma = codigoTurma;
        }

        public string CodigoTurma { get; set; }
    }
}
