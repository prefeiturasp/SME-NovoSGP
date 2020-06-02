using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Github.ObterVersaoRelease
{
    public class ObterUltimaVersaoQueryHandler : IRequestHandler<ObterUltimaVersaoQuery, string>
    {
        private readonly IServicoGithub servicoGithub;
        private readonly IRepositorioCache repositorioCache;

        public ObterUltimaVersaoQueryHandler(IServicoGithub servicoGithub, IRepositorioCache repositorioCache)
        {
            this.servicoGithub = servicoGithub ?? throw new ArgumentNullException(nameof(servicoGithub));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<string> Handle(ObterUltimaVersaoQuery request, CancellationToken cancellationToken)
        {
            var versao = await repositorioCache.ObterAsync("versao", false);
            if (string.IsNullOrWhiteSpace(versao))
            {
                string numeroVersao = await servicoGithub.RecuperarUltimaVersao();
                await repositorioCache.SalvarAsync("versao", numeroVersao, 1080, false);
                return numeroVersao;
            }
            return versao;
        }
    }
}
