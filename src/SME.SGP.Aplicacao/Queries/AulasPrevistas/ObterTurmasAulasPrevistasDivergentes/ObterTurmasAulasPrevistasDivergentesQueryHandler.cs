using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasAulasPrevistasDivergentesQueryHandler : IRequestHandler<ObterTurmasAulasPrevistasDivergentesQuery, IEnumerable<RegistroAulaPrevistaDivergenteDto>>
    {
        private readonly IRepositorioNotificacaoAulaPrevista repositorioNotificacaoAulaPrevista;

        public ObterTurmasAulasPrevistasDivergentesQueryHandler(IRepositorioNotificacaoAulaPrevista repositorioNotificacaoAulaPrevista)
        {
            this.repositorioNotificacaoAulaPrevista = repositorioNotificacaoAulaPrevista ?? throw new System.ArgumentNullException(nameof(repositorioNotificacaoAulaPrevista));
        }

        public async Task<IEnumerable<RegistroAulaPrevistaDivergenteDto>> Handle(ObterTurmasAulasPrevistasDivergentesQuery request, CancellationToken cancellationToken)
            => await repositorioNotificacaoAulaPrevista.ObterTurmasAulasPrevistasDivergentes(request.QuantidadeDiasBimestreNotificacao);
    }
}
