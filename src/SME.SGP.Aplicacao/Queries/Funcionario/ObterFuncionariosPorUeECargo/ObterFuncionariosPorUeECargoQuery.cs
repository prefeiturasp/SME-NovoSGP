using MediatR;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFuncionariosPorUeECargoQuery : IRequest<IEnumerable<FuncionarioDTO>>
    {
        public string CodigoUE;
        public int CodigoCargo;

        public ObterFuncionariosPorUeECargoQuery(string codigoUE, int codigoCargo)
        {
            this.CodigoUE = codigoUE;
            this.CodigoCargo = codigoCargo;
        }
    }
}