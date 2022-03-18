using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasPorIdsQueryHandler : IRequestHandler<ObterAulasPorIdsQuery, IEnumerable<Aula>>
    {
        private readonly IRepositorioAulaConsulta repositorioAulaConsulta;

        public ObterAulasPorIdsQueryHandler(IRepositorioAulaConsulta repositorioAulaConsulta)
        {
            this.repositorioAulaConsulta = repositorioAulaConsulta ?? throw new ArgumentNullException(nameof(repositorioAulaConsulta));
        }

        public async Task<IEnumerable<Aula>> Handle(ObterAulasPorIdsQuery request, CancellationToken cancellationToken)
            => await repositorioAulaConsulta.ObterAulasPorIds(request.AulasIds);
    }
}
