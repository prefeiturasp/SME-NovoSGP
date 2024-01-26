using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterResponsaveisAtribuidosUeTiposQueryHandler : IRequestHandler<ObterResponsaveisAtribuidosUeTiposQuery, IEnumerable<SupervisorEscolasDreDto>>
    {
        private readonly IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre;

        public ObterResponsaveisAtribuidosUeTiposQueryHandler(IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre)
        {
            this.repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre ?? throw new ArgumentNullException(nameof(repositorioSupervisorEscolaDre));
        }

        public async Task<IEnumerable<SupervisorEscolasDreDto>> Handle(ObterResponsaveisAtribuidosUeTiposQuery request, CancellationToken cancellationToken)
            => await repositorioSupervisorEscolaDre.ObterResponsaveisPorDreUeTiposAtribuicaoAsync(string.Empty, string.Empty, request.TiposResponsavelAtribuicao);
    }
}
