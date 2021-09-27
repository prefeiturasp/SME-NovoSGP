using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

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
