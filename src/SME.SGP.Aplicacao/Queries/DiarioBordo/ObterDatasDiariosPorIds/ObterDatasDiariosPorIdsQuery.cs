using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterDatasDiariosPorIdsQuery : IRequest<IEnumerable<DateTime>>
    {
        public ObterDatasDiariosPorIdsQuery(List<long> diariosBordoIds)
        {
            this.DiariosBordoIds = diariosBordoIds;
        }

        public IEnumerable<long> DiariosBordoIds { get; }
    }
}
