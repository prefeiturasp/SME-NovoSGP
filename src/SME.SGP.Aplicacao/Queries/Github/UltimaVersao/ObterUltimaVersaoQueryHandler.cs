using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Github.ObterVersaoRelease
{
    public class ObterUltimaVersaoQueryHandler : IRequestHandler<ObterUltimaVersaoQuery, string>
    {
        private readonly IServicoGithub servicoGithub;

        public ObterUltimaVersaoQueryHandler(IServicoGithub servicoGithub)
        {
            this.servicoGithub = servicoGithub ?? throw new ArgumentNullException(nameof(servicoGithub));
        }

        public async Task<string> Handle(ObterUltimaVersaoQuery request, CancellationToken cancellationToken)
        {
            return await servicoGithub.RecuperarUltimaVersao();
        }
    }
}
