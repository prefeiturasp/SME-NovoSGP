using MediatR;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFuncionariosPorUeEFuncaoExternaQuery : IRequest<IEnumerable<FuncionarioDTO>>
    {
        public string CodigoUE;
        public int CodigoFuncaoExterna;

        public ObterFuncionariosPorUeEFuncaoExternaQuery(string codigoUE, int codigoFuncaoExterna)
        {
            this.CodigoUE = codigoUE;
            this.CodigoFuncaoExterna = codigoFuncaoExterna;
        }
    }
}