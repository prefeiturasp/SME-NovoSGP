using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasPorCodigosQuery : IRequest<IEnumerable<Turma>>
    {
        public ObterTurmasPorCodigosQuery(string[] codigos)
        {
            Codigos = codigos;
        }

        public string[] Codigos { get; set; }
    }
}
