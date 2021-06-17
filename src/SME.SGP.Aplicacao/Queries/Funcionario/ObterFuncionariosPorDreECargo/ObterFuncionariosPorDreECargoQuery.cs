using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    internal class ObterFuncionariosPorDreECargoQuery : IRequest<IEnumerable<UsuarioEolRetornoDto>>
    {
        public string CodigoDRE;
        public int CodigoCargo;

        public ObterFuncionariosPorDreECargoQuery(string codigoDRE, int codigoCargo)
        {
            this.CodigoDRE = codigoDRE;
            this.CodigoCargo = codigoCargo;
        }
    }
}