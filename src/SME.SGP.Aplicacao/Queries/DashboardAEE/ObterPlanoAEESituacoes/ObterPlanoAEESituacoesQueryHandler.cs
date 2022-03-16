using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanoAEESituacoesQueryHandler : IRequestHandler<ObterPlanoAEESituacoesQuery, IEnumerable<AEESituacaoPlanoDto>>
    {
        private readonly IRepositorioPlanoAEEConsulta repositorio;

        public ObterPlanoAEESituacoesQueryHandler(IRepositorioPlanoAEEConsulta repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<AEESituacaoPlanoDto>> Handle(ObterPlanoAEESituacoesQuery request, CancellationToken cancellationToken)
            => await repositorio.ObterQuantidadeSituacoes(request.Ano, request.DreId, request.UeId);
    }
}
