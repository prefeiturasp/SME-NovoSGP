using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SME.SGP.Aplicacao.Configuracoes;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace SME.SGP.Aplicacao.Servicos
{
    public class ServicoTokenJwt : IServicoTokenJwt
    {
        private readonly IOptionsMonitor<JwtTokenSettings> jwtTokenSettings;

        public ServicoTokenJwt(IOptionsMonitor<JwtTokenSettings> jwtTokenSettings)
        {
            this.jwtTokenSettings = jwtTokenSettings ?? throw new ArgumentNullException(nameof(jwtTokenSettings));
        }

        public string GerarToken(string usuarioId, string usuarioLogin, string nome, Guid[] perfis)
        {
            IEnumerable<Claim> claims = new List<Claim>();
            var now = DateTime.Now;
            var token = new JwtSecurityToken(
                issuer: jwtTokenSettings.CurrentValue.Issuer,
                audience: jwtTokenSettings.CurrentValue.Audience,
                claims: claims
                           .Append(new Claim("Id", usuarioId))
                           .Append(new Claim(ClaimTypes.Name, usuarioLogin))
                           .Append(new Claim("UserName", nome))
                           .Append(new Claim("Perfis", string.Join(",", perfis))),
                notBefore: now,
                expires: now.AddMinutes(jwtTokenSettings.CurrentValue.ExpiresInMinutes),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(jwtTokenSettings.CurrentValue.IssuerSigningKey)),
                        SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler()
                      .WriteToken(token);
        }
    }
}