using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRespostasPlanoAEEPorVersaoQueryHandler : IRequestHandler<ObterRespostasPlanoAEEPorVersaoQuery, IEnumerable<RespostaQuestaoDto>>
    {
        private readonly IRepositorioPlanoAEEResposta repositorioPlanoAEEResposta;

        public ObterRespostasPlanoAEEPorVersaoQueryHandler(IRepositorioPlanoAEEResposta repositorioPlanoAEEResposta)
        {
            this.repositorioPlanoAEEResposta = repositorioPlanoAEEResposta ?? throw new ArgumentNullException(nameof(repositorioPlanoAEEResposta));
        }

        public async Task<IEnumerable<RespostaQuestaoDto>> Handle(ObterRespostasPlanoAEEPorVersaoQuery request, CancellationToken cancellationToken)
            => await repositorioPlanoAEEResposta.ObterRespostasPorVersaoPlano(request.VersaoPlanoId);
    }
}
