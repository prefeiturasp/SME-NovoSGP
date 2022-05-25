using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterResponsaveisPorDreQueryHandler : IRequestHandler<ObterResponsaveisPorDreQuery, IEnumerable<SupervisorEscolasDreDto>>
    {
        private readonly IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre;

        public ObterResponsaveisPorDreQueryHandler(IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre)
        {
            this.repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre ?? throw new ArgumentNullException(nameof(repositorioSupervisorEscolaDre));
        }

        public async Task<IEnumerable<SupervisorEscolasDreDto>> Handle(ObterResponsaveisPorDreQuery request, CancellationToken cancellationToken)
            => await repositorioSupervisorEscolaDre.ObtemSupervisoresPorDreAsync(request.CodigoDre, request.TipoResponsavelAtribuicao);
    }
}
