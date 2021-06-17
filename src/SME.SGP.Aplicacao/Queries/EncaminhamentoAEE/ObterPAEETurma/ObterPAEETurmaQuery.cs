using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterPAEETurmaQuery : IRequest<IEnumerable<UsuarioEolRetornoDto>>
    {
        public ObterPAEETurmaQuery(string codigoDRE, string codigoUE)
        {
            CodigoDRE = codigoDRE;
            CodigoUE = codigoUE;
        }

        public string CodigoDRE { get; }
        public string CodigoUE { get; }
    }
}
