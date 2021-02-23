using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterVersoesPlanoAEESemReestruturacaoQueryHandler : IRequestHandler<ObterVersoesPlanoAEESemReestruturacaoQuery, IEnumerable<PlanoAEEVersaoDto>>
    {
        private readonly IRepositorioPlanoAEEVersao repositorioPlanoAEEVersao;

        public ObterVersoesPlanoAEESemReestruturacaoQueryHandler(IRepositorioPlanoAEEVersao repositorioPlanoAEEVersao)
        {
            this.repositorioPlanoAEEVersao = repositorioPlanoAEEVersao ?? throw new ArgumentNullException(nameof(repositorioPlanoAEEVersao));
        }

        public async Task<IEnumerable<PlanoAEEVersaoDto>> Handle(ObterVersoesPlanoAEESemReestruturacaoQuery request, CancellationToken cancellationToken)
            => await repositorioPlanoAEEVersao.ObterVersoesSemReestruturacaoPorPlanoId(request.PlanoId, request.ReestruturacaoId);
    }
}
