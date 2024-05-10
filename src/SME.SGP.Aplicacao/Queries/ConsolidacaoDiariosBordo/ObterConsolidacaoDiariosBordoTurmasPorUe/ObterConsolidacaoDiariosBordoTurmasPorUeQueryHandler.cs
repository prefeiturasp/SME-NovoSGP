using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterConsolidacaoDiariosBordoTurmasPorUeQueryHandler : IRequestHandler<ObterConsolidacaoDiariosBordoTurmasPorUeQuery, IEnumerable<ConsolidacaoDiariosBordo>>
    {
        private readonly IRepositorioConsolidacaoDiariosBordo repositorio;

        public ObterConsolidacaoDiariosBordoTurmasPorUeQueryHandler(IRepositorioConsolidacaoDiariosBordo repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<ConsolidacaoDiariosBordo>> Handle(ObterConsolidacaoDiariosBordoTurmasPorUeQuery request, CancellationToken cancellationToken)
            => await repositorio.GerarConsolidacaoPorUe(request.UeId, DateTime.Now.Year);
    }
}
