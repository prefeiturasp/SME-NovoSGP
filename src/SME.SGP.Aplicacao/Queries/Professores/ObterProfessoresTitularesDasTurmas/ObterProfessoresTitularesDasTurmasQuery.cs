using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterProfessoresTitularesDasTurmasQuery: IRequest<IEnumerable<string>>
    {
        public ObterProfessoresTitularesDasTurmasQuery(IEnumerable<string> codigosTurmas)
        {
            CodigosTurmas = codigosTurmas;
        }

        public IEnumerable<string> CodigosTurmas { get; }
    }
}
