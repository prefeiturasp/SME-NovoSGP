using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAnosLetivosPAPQuery : IRequest<IEnumerable<ObterAnoLetivoPAPRetornoDto>>
    {
        public int AnoAtual { get; set; }
    }
}
