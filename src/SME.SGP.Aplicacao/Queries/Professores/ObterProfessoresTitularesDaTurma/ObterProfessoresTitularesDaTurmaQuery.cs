using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterProfessoresTitularesDaTurmaQuery : IRequest<IEnumerable<string>>
    {
        public ObterProfessoresTitularesDaTurmaQuery(string codigoTurma)
        {
            CodigoTurma = codigoTurma;
        }

        public string CodigoTurma { get; set; }
    }
}
