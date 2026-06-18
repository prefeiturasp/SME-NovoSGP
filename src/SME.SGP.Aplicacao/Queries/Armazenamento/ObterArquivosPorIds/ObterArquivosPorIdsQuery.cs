using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

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