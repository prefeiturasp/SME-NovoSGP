using MediatR;
using SME.SGP.Infra.Contexto;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPerfilAtualQueryHandler : IRequestHandler<ObterPerfilAtualQuery, Guid>
    {
        private const string CLAIM_PERFIL_ATUAL = "perfil";
        private readonly IMediator mediator;
        private readonly IContextoAplicacao contextoAplicacao;

        public ObterPerfilAtualQueryHandler(IMediator mediator, IContextoAplicacao contextoAplicacao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.contextoAplicacao = contextoAplicacao ?? throw new ArgumentNullException(nameof(contextoAplicacao));

        }

        public async Task<Guid> Handle(ObterPerfilAtualQuery request, CancellationToken cancellationToken)
        {
            var tokenAtual = await mediator.Send(ObterTokenAtualQuery.Instance);
            var perfil = Guid.Empty;

            if (!string.IsNullOrEmpty(tokenAtual))
                perfil = Guid.Parse(ObterClaims(tokenAtual)
                    .FirstOrDefault(claim => claim.Type == "perfil")?.Value
                    ?? string.Empty);
            else
                perfil = Guid.Parse(ObterClaim(CLAIM_PERFIL_ATUAL) ?? string.Empty);

            return perfil;
        }

        private IEnumerable<Claim> ObterClaims(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            return tokenS.Claims;
        }

        public string ObterClaim(string nomeClaim)
        {
            var claim = contextoAplicacao.ObterVariavel<IEnumerable<InternalClaim>>("Claims").FirstOrDefault(a => a.Type == nomeClaim);
            return claim?.Value;
        }

    }
}
