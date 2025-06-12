using MediatR;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFuncionariosPorUeECargoQuery : IRequest<IEnumerable<FuncionarioDTO>>
    {
        public ObterFuncionariosPorUeECargoQuery(string codigoUE, int codigoCargo)
        {
            this.CodigoUE = codigoUE;
            this.CodigoCargo = codigoCargo;
        }

        public string CodigoUE { get; set; }
        public int CodigoCargo { get; set; }
    }
}