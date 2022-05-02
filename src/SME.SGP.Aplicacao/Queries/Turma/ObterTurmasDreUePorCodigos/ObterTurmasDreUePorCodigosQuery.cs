using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasDreUePorCodigosQuery : IRequest<IEnumerable<Turma>>
    {
        public ObterTurmasDreUePorCodigosQuery(string[] codigos)
        {
            Codigos = codigos;
        }

        public string[] Codigos { get; set; }
    }
}
