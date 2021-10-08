using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalRIsPorDreQueryHandler : IRequestHandler<ObterTotalRIsPorDreQuery, IEnumerable<GraficoBaseDto>>
    {
        private readonly IRepositorioRegistroIndividual repositorioRegistroIndividual;

        public ObterTotalRIsPorDreQueryHandler(IRepositorioRegistroIndividual repositorioRegistroIndividual)
        {
            this.repositorioRegistroIndividual = repositorioRegistroIndividual ?? throw new ArgumentNullException(nameof(repositorioRegistroIndividual));
        }

        public async Task<IEnumerable<GraficoBaseDto>> Handle(ObterTotalRIsPorDreQuery request, CancellationToken cancellationToken)
            => await repositorioRegistroIndividual.ObterTotalPorDRE(request.AnoLetivo, request.Ano);
    }
}
