using MediatR;
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
        private readonly IMediator mediator;

        public ObterPerfilAtualQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<Guid> Handle(ObterPerfilAtualQuery request, CancellationToken cancellationToken)
        {
            var tokenAtual = await mediator.Send(new ObterTokenAtualQuery());
            var perfil = Guid.Parse(ObterClaims(tokenAtual)
                .FirstOrDefault(claim => claim.Type == "perfil")?.Value
                ?? string.Empty);

            return perfil;
        }

        private IEnumerable<Claim> ObterClaims(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            return tokenS.Claims;
        }

    }
}
