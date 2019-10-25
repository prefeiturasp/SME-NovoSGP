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

        public string GerarToken(string usuarioLogin, string codigoRf, IEnumerable<Permissao> permissionamentos, string guidPerfil)
        {
            IList<Claim> claims = new List<Claim>();

            claims.Add(new Claim("login", usuarioLogin));
            claims.Add(new Claim("rf", codigoRf));
            claims.Add(new Claim("perfilAtual", guidPerfil));

            foreach (var permissao in permissionamentos)
            {
                claims.Add(new Claim("roles", permissao.ToString()));
            }

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