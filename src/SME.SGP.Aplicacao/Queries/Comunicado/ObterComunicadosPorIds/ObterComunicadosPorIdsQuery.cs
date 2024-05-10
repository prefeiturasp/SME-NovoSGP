using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterComunicadosPorIdsQuery : IRequest<IEnumerable<Comunicado>>
    {
        public ObterComunicadosPorIdsQuery(long[] ids)
        {
            Ids = ids;
        }

        public long[] Ids { get; set; }
    }
}
