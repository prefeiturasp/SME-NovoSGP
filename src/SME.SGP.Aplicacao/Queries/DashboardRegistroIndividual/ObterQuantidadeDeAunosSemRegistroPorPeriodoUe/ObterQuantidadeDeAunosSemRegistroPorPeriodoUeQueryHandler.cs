using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeDeAunosSemRegistroPorPeriodoUeQueryHandler : IRequestHandler<ObterQuantidadeDeAunosSemRegistroPorPeriodoUeQuery, IEnumerable<GraficoBaseDto>>
    {
        private readonly IRepositorioRegistroIndividual repositorioRegistroIndividual;

        public ObterQuantidadeDeAunosSemRegistroPorPeriodoUeQueryHandler(IRepositorioRegistroIndividual repositorioRegistroIndividual)
        {
            this.repositorioRegistroIndividual = repositorioRegistroIndividual ?? throw new ArgumentNullException(nameof(repositorioRegistroIndividual));
        }

        public async Task<IEnumerable<GraficoBaseDto>> Handle(ObterQuantidadeDeAunosSemRegistroPorPeriodoUeQuery request, CancellationToken cancellationToken)
        {
            return await repositorioRegistroIndividual.ObterQuantidadeDeAunosSemRegistroPorPeriodoUeAsync(request.AnoLetivo, request.UeId, request.Modalidade, request.DataInicial);
        }
    }
}
