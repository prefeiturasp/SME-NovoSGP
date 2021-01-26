using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterSupervisoresPorUeQueryHandler : IRequestHandler<ObterSupervisoresPorUeQuery, IEnumerable<SupervisorEscolasDreDto>>
    {
        private readonly IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre;

        public ObterSupervisoresPorUeQueryHandler(IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre)
        {
            this.repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre ?? throw new ArgumentNullException(nameof(repositorioSupervisorEscolaDre));
        }

        public async Task<IEnumerable<SupervisorEscolasDreDto>> Handle(ObterSupervisoresPorUeQuery request, CancellationToken cancellationToken)
                      => await repositorioSupervisorEscolaDre.ObtemSupervisoresPorUeAsync(request.CodigoUe);
    }
}
