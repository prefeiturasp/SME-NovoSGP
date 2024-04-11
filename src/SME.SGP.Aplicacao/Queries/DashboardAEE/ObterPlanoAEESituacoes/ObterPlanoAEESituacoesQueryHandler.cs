using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanoAEESituacoesQueryHandler : IRequestHandler<ObterPlanoAEESituacoesQuery, DashboardAEEPlanosSituacaoDto>
    {
        private readonly IRepositorioPlanoAEEConsulta repositorio;

        public ObterPlanoAEESituacoesQueryHandler(IRepositorioPlanoAEEConsulta repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<DashboardAEEPlanosSituacaoDto> Handle(ObterPlanoAEESituacoesQuery request, CancellationToken cancellationToken)
            => await repositorio.ObterDashBoardPlanosSituacoes(request.Ano, request.DreId, request.UeId);
    }
}
