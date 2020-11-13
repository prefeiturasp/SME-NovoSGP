using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterParametrosSistemaPorTiposQuery : IRequest<IEnumerable<ParametrosSistema>>
    {
        public long[] Tipos { get; set; }
    }
}
