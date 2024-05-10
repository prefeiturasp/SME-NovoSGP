using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuariosIdPorCodigoUeQuery : IRequest<IEnumerable<long>>
    {
        public ObterUsuariosIdPorCodigoUeQuery(string codigoUe)
        {
            CodigoUe = codigoUe;
        }

        public string CodigoUe { get; set; }
    }
}
