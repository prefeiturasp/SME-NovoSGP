using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFuncionariosCompletosPorUeECargoQuery : IRequest<IEnumerable<UsuarioEolRetornoDto>>
    {
        public ObterFuncionariosCompletosPorUeECargoQuery(string ueCodigo, int cargoCodigo)
        {
            UeCodigo = ueCodigo;
            CargoCodigo = cargoCodigo;
        }

        public string UeCodigo { get; set; }
        public int CargoCodigo { get; set; }
    }
}
