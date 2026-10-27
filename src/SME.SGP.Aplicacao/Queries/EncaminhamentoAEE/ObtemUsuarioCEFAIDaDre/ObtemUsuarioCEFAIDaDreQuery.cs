using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObtemUsuarioCEFAIDaDreQuery : IRequest<IEnumerable<long>>
    {
        public ObtemUsuarioCEFAIDaDreQuery(string codigoDRE)
        {
            CodigoDRE = codigoDRE;
        }

        public string CodigoDRE { get; }
    }
}
