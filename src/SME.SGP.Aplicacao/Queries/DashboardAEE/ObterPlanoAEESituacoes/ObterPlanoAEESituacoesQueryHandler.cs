using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanoAEESituacoesQueryHandler : IRequestHandler<ObterPlanoAEESituacoesQuery, IEnumerable<AEESituacaoPlanoDto>>
    {
        private readonly IRepositorioPlanoAEE repositorio;

        public ObterPlanoAEESituacoesQueryHandler(IRepositorioPlanoAEE repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<AEESituacaoPlanoDto>> Handle(ObterPlanoAEESituacoesQuery request, CancellationToken cancellationToken)
            => await repositorio.ObterQuantidadeSituacoes(request.Ano, request.DreId, request.UeId);
    }
}
