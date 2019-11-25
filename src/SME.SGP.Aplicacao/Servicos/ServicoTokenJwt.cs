using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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

        public string GerarToken(IEnumerable<Claim> claims)
        {
            var now = DateTime.Now;
            var token = new JwtSecurityToken(
                issuer: configuration.GetSection("JwtTokenSettings:Issuer").Value,
                audience: configuration.GetSection("JwtTokenSettings:Audience").Value,
                claims: claims,
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

        public bool TemPerfilNoToken(string guid)
        {
            throw new NotImplementedException();
        }
    }
}