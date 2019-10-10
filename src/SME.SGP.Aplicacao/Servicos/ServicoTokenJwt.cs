using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SME.SGP.Dominio;
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
        private readonly IHttpContextAccessor httpContextAccessor;

        public ServicoTokenJwt(IConfiguration configuration, IHttpContextAccessor httpContext)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.httpContextAccessor = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
        }

        public string GerarToken(string usuarioLogin, IEnumerable<Permissao> permissionamentos)
        {
            IList<Claim> claims = new List<Claim>();

            claims.Add(new Claim("login", usuarioLogin));

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

        public string ObterLoginAtual()
        {
            var loginAtual = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(a => a.Type == "login");
            if (loginAtual == null)
                throw new NegocioException("Não foi possível localizar o login no token");

            return loginAtual.Value;
        }

        public bool TemPerfilNoToken(string guid)
        {
            throw new NotImplementedException();
        }
    }
}