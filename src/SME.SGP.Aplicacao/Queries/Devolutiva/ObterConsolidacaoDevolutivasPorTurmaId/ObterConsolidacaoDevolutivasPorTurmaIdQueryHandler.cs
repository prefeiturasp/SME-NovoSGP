using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterConsolidacaoDevolutivasPorTurmaIdQueryHandler : IRequestHandler<ObterConsolidacaoDevolutivasPorTurmaIdQuery, ConsolidacaoDevolutivas>
    {
        private readonly IRepositorioConsolidacaoDevolutivasConsulta repositorioConsolidacaoDevolutivas;

        public ObterConsolidacaoDevolutivasPorTurmaIdQueryHandler(IRepositorioConsolidacaoDevolutivasConsulta repositorioConsolidacaoDevolutivas)
        {
            this.repositorioConsolidacaoDevolutivas = repositorioConsolidacaoDevolutivas ?? throw new ArgumentNullException(nameof(repositorioConsolidacaoDevolutivas));
        }

        public async Task<ConsolidacaoDevolutivas> Handle(ObterConsolidacaoDevolutivasPorTurmaIdQuery request, CancellationToken cancellationToken)
            => await repositorioConsolidacaoDevolutivas.ObterConsolidacaoDevolutivasPorTurmaId(request.TurmaId);
    }
}
