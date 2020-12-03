using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterSupervisoresPorDreQueryHandler : IRequestHandler<ObterSupervisoresPorDreQuery, IEnumerable<SupervisorEscolasDreDto>>
    {
        private readonly IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre;

        public ObterSupervisoresPorDreQueryHandler(IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre)
        {
            this.repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre ?? throw new ArgumentNullException(nameof(repositorioSupervisorEscolaDre));
        }

        public async Task<IEnumerable<SupervisorEscolasDreDto>> Handle(ObterSupervisoresPorDreQuery request, CancellationToken cancellationToken)
                      => await repositorioSupervisorEscolaDre.ObtemSupervisoresPorDreAsync(request.CodigoDre);
    }
}
