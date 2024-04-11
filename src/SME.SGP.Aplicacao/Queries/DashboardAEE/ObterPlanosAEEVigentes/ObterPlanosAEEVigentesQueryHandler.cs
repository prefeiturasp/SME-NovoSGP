using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanosAEEVigentesQueryHandler : IRequestHandler<ObterPlanosAEEVigentesQuery, DashboardAEEPlanosVigentesDto>
    {
        private readonly IRepositorioPlanoAEEConsulta repositorio;

        public ObterPlanosAEEVigentesQueryHandler(IRepositorioPlanoAEEConsulta repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<DashboardAEEPlanosVigentesDto> Handle(ObterPlanosAEEVigentesQuery request, CancellationToken cancellationToken)
            => await repositorio.ObterDashboardPlanosVigentes(request.Ano, request.DreId, request.UeId);
    }
}
