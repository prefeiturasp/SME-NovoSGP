using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtualizarSituacaoPlanoAEEPorVersaoCommandHandler : IRequestHandler<AtualizarSituacaoPlanoAEEPorVersaoCommand, bool>
    {
        private readonly IRepositorioPlanoAEE repositorioPlanoAEE;

        public AtualizarSituacaoPlanoAEEPorVersaoCommandHandler(IRepositorioPlanoAEE repositorioPlanoAEE)
        {
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
        }

        public async Task<bool> Handle(AtualizarSituacaoPlanoAEEPorVersaoCommand request, CancellationToken cancellationToken)
            => (await repositorioPlanoAEE.AtualizarSituacaoPlanoPorVersao(request.VersaoId, (int)request.Situacao)) > 0;
    }
}
