using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentoAEESituacoesQueryHandler : IRequestHandler<ObterEncaminhamentoAEESituacoesQuery, DashboardAEEEncaminhamentosDto>
    {
        private readonly IRepositorioEncaminhamentoAEE repositorio;

        public ObterEncaminhamentoAEESituacoesQueryHandler(IRepositorioEncaminhamentoAEE repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<DashboardAEEEncaminhamentosDto> Handle(ObterEncaminhamentoAEESituacoesQuery request, CancellationToken cancellationToken)
            => await repositorio.ObterDashBoardAEEEncaminhamentos(request.Ano, request.DreId, request.UeId);
    }
}
