using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    internal class ObterFuncionariosPorDreECargoQuery : IRequest<IEnumerable<UsuarioEolRetornoDto>>
    {
        public ObterFuncionariosPorDreECargoQuery(string codigoDRE, int codigoCargo)
        {
            CodigoDRE = codigoDRE;
            CodigoCargo = codigoCargo;
        }

        public string CodigoDRE { get; set; }
        public int CodigoCargo { get; set; }

    }
}