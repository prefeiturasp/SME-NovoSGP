using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPareceresConclusivosPorTurmaQueryHandler : IRequestHandler<ObterPareceresConclusivosPorTurmaQuery, IEnumerable<ConselhoClasseParecerConclusivo>>
    {
        private readonly IRepositorioConselhoClasseParecerConclusivo repositorioParecer;

        public ObterPareceresConclusivosPorTurmaQueryHandler(IRepositorioConselhoClasseParecerConclusivo repositorioParecer)
        {
            this.repositorioParecer = repositorioParecer ?? throw new ArgumentNullException(nameof(repositorioParecer));
        }

        public async Task<IEnumerable<ConselhoClasseParecerConclusivo>> Handle(ObterPareceresConclusivosPorTurmaQuery request, CancellationToken cancellationToken)
            => await repositorioParecer.ObterListaPorTurmaIdAsync(request.TurmaId, DateTime.Today);
    }
}
