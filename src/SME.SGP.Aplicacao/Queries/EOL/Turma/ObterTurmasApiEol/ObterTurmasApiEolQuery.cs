using MediatR;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasApiEolQuery : IRequest<IList<TurmaApiEolDto>>
    {
        public ObterTurmasApiEolQuery(List<string> codigosTurmas)
        {
            CodigosTurmas = codigosTurmas;
        }

        public List<string> CodigosTurmas { get; set; }
    }
}
