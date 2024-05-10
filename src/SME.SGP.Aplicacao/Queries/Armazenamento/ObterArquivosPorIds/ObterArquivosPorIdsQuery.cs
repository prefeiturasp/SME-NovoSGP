using System.Collections.Generic;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterArquivosPorIdsQuery : IRequest<IEnumerable<Arquivo>>
    {
        public ObterArquivosPorIdsQuery(long[] ids)
        {
            Ids = ids;
        }

        public long[] Ids { get; }
    }
}