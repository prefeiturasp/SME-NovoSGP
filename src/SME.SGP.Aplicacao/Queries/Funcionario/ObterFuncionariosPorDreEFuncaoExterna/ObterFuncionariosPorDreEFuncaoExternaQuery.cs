using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFuncionariosPorDreEFuncaoExternaQuery : IRequest<IEnumerable<UsuarioEolRetornoDto>>
    {
        public ObterFuncionariosPorDreEFuncaoExternaQuery(string codigoDRE, int codigoFuncaoExterna)
        {
            CodigoDRE = codigoDRE;
            CodigoFuncaoExterna = codigoFuncaoExterna;
        }

        public string CodigoDRE { get; set; }
        public int CodigoFuncaoExterna { get; set; }

    }
}