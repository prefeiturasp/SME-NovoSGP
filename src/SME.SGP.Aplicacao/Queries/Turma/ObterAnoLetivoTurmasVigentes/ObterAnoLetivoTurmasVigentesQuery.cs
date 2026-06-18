using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAnoLetivoTurmasVigentesQuery : IRequest<IEnumerable<int>>
    {
        public string UeCodigo { get; set; }
        public ObterAnoLetivoTurmasVigentesQuery(string codigoUe)
        {
            UeCodigo = codigoUe;
        }
    }
}
