using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

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
