using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterInfoPedagogicasComponentesCurricularesPorIdsQuery : IRequest<IEnumerable<InfoComponenteCurricular>>
    {
        public ObterInfoPedagogicasComponentesCurricularesPorIdsQuery()
        { }

        public ObterInfoPedagogicasComponentesCurricularesPorIdsQuery(long[] ids)
        {
            Ids = ids;
        }

        public ObterInfoPedagogicasComponentesCurricularesPorIdsQuery(long id)
        {
            Ids = new long[] { id };
        }

        public long[] Ids { get; set; }
    }

}
