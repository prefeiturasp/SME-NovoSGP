using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterResponsaveisAtribuidosUePorDreUeTiposQueryHandler : IRequestHandler<ObterResponsaveisAtribuidosUePorDreUeTiposQuery, IEnumerable<SupervisorEscolasDreDto>>
    {
        private readonly IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre;

        public ObterResponsaveisAtribuidosUePorDreUeTiposQueryHandler(IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre)
        {
            this.repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre ?? throw new ArgumentNullException(nameof(repositorioSupervisorEscolaDre));
        }

        public async Task<IEnumerable<SupervisorEscolasDreDto>> Handle(ObterResponsaveisAtribuidosUePorDreUeTiposQuery request, CancellationToken cancellationToken)
            => await repositorioSupervisorEscolaDre.ObterResponsaveisPorDreUeTiposAtribuicaoAsync(request.CodigoDre, request.CodigoUe, request.TiposResponsavelAtribuicao);
    }
}
