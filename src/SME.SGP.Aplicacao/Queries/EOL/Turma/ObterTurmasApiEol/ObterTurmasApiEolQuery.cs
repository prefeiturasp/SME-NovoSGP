using System.Collections.Generic;
using MediatR;
using SME.SGP.Infra.Dtos;

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
