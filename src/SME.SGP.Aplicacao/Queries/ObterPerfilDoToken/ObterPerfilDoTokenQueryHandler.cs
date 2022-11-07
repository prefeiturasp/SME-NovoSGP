using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ObterPerfilDoTokenQueryHandler : IRequestHandler<ObterPerfilDoTokenQuery, Guid>
    {
        private readonly IContextoAplicacao contextoAplicacao;

        public ObterPerfilDoTokenQueryHandler(IContextoAplicacao contextoAplicacao)
        {
            this.contextoAplicacao = contextoAplicacao ?? throw new ArgumentNullException(nameof(contextoAplicacao));
        }

        public async Task<Guid> Handle(ObterPerfilDoTokenQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(Guid.Parse(
                ObterClaims(ObterTokenAtual()).FirstOrDefault(claim => claim.Type == "perfil")?.Value ?? string.Empty));
        }
    
        private static IEnumerable<Claim> ObterClaims(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;
            return tokenS?.Claims;
        }    
    
        private string ObterTokenAtual()
        {
            return contextoAplicacao.ObterVariavel<string>("TokenAtual");
        }    
    }
}