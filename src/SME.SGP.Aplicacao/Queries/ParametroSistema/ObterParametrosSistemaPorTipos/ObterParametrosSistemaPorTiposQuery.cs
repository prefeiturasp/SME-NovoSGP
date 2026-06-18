using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterParametrosSistemaPorTiposQuery : IRequest<IEnumerable<ParametrosSistema>>
    {
        public long[] Tipos { get; set; }
    }
}
