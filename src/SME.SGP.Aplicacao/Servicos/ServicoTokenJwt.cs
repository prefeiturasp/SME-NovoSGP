using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace SME.SGP.Aplicacao.Servicos
{
    public class ServicoTokenJwt : IServicoTokenJwt
    {
        private readonly IConfiguration configuration;

        public ServicoTokenJwt(IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public string GerarToken(string usuarioId, string usuarioLogin, string nome, Guid[] perfis)
        {
            IEnumerable<Claim> claims = new List<Claim>();
            var now = DateTime.Now;
            var token = new JwtSecurityToken(
                issuer: configuration.GetSection("JwtTokenSettings:Issuer").Value,
                audience: configuration.GetSection("JwtTokenSettings:Audience").Value,
                claims: claims
                           .Append(new Claim("Id", usuarioId))
                           .Append(new Claim(ClaimTypes.Name, usuarioLogin))
                           .Append(new Claim("UserName", nome))
                           .Append(new Claim("Perfis", perfis == null ? string.Empty : string.Join(",", perfis))),
                notBefore: now,
                expires: now.AddMinutes(double.Parse(configuration.GetSection("JwtTokenSettings:ExpiresInMinutes").Value)),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(configuration.GetSection("JwtTokenSettings:IssuerSigningKey").Value)),
                        SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler()
                      .WriteToken(token);
        }
    }
}