using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUltimaVersaoPlanoAEEQueryHandler : IRequestHandler<ObterUltimaVersaoPlanoAEEQuery, PlanoAEEVersaoDto>
    {
        private readonly IRepositorioPlanoAEEVersao repositorioPlanoAEEVersao;

        public ObterUltimaVersaoPlanoAEEQueryHandler(IRepositorioPlanoAEEVersao repositorioPlanoAEEVersao)
        {
            this.repositorioPlanoAEEVersao = repositorioPlanoAEEVersao ?? throw new ArgumentNullException(nameof(repositorioPlanoAEEVersao));
        }

        public async Task<PlanoAEEVersaoDto> Handle(ObterUltimaVersaoPlanoAEEQuery request, CancellationToken cancellationToken)
            => await repositorioPlanoAEEVersao.ObterUltimaVersaoPorPlanoId(request.PlanoId);
    }
}
