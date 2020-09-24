using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorIdsQuery : IRequest<IEnumerable<ComponenteCurricular>>
    {
        public ObterComponentesCurricularesPorIdsQuery(long[] ids)
        {
            this.Ids = ids;
        }

        public long[] Ids { get; set; }
    }
}
