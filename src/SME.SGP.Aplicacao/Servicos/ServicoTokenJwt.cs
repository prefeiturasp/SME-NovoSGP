using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
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
        private readonly IContextoAplicacao contextoAplicacao;
        private string tokenGerado;

        public ServicoTokenJwt(IConfiguration configuration,
                               IContextoAplicacao  contextoAplicacao)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.contextoAplicacao = contextoAplicacao ?? throw new ArgumentNullException(nameof(contextoAplicacao));
        }

        public string GerarToken(string usuarioLogin, string usuarioNome, string codigoRf, Guid guidPerfil, IEnumerable<Permissao> permissionamentos)
        {
            IList<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Name, usuarioLogin));
            claims.Add(new Claim("login", usuarioLogin));
            claims.Add(new Claim("nome", usuarioNome));
            claims.Add(new Claim("rf", codigoRf ?? string.Empty));
            claims.Add(new Claim("perfil", guidPerfil.ToString()));

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

            tokenGerado = new JwtSecurityTokenHandler()
                      .WriteToken(token);

            //SalvarToken(usuarioLogin);

            return tokenGerado;
        }

        public DateTime ObterDataHoraCriacao()
            => ObterDataHoraCriacao(ObterTokenAtual());

        public DateTime ObterDataHoraExpiracao()
        {
            var tokenStr = ObterTokenAtual();
            if (!string.IsNullOrEmpty(tokenStr))
            {
                var token = (new JwtSecurityTokenHandler()).ReadToken(tokenStr) as JwtSecurityToken;
                return token.ValidTo;
            }
            return DateTime.MinValue;
        }

        public string ObterLogin()
            => ObterLoginDoToken(ObterTokenAtual());

        public Guid ObterPerfil()
            => ObterPerfilDoToken(ObterTokenAtual());

        public bool TemPerfilNoToken(string guid)
        {
            throw new NotImplementedException();
        }

        public bool TokenAtivo()
            => TokenAtivo(ObterTokenAtual());

        public bool TokenPresente()
        {
            return contextoAplicacao.ObterVarivel<bool>("TemAuthorizationHeader");
        }

        private string ObterTokenAtual()
        {
            // Obtem token gerado ou o token da autenticação do contexto
            if (!string.IsNullOrEmpty(tokenGerado))
                return tokenGerado;

            return contextoAplicacao.ObterVarivel<string>("TokenAtual");
        }

        #region Private Methods

        private string ObterChave(string nome)
            => $"token:{nome}";

        private string ObterChaveLogin(string usuarioLogin)
            => ObterChave(usuarioLogin);

        private string ObterChaveToken(string token)
            => ObterChave(ObterLoginDoToken(token));

        private IEnumerable<Claim> ObterClaims(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            return tokenS.Claims;
        }

        private DateTime ObterDataHoraCriacao(string tokenStr)
        {
            if (!string.IsNullOrEmpty(tokenStr))
            {
                var token = (new JwtSecurityTokenHandler()).ReadToken(tokenStr) as JwtSecurityToken;
                return token.ValidFrom;
            }

            return DateTime.MinValue;
        }

        private string ObterLoginDoToken(string token)
            => ObterClaims(token)
                .FirstOrDefault(claim => claim.Type == "login")?.Value ?? string.Empty;

        private Guid ObterPerfilDoToken(string token)
            => Guid.Parse(ObterClaims(token)
                .FirstOrDefault(claim => claim.Type == "perfil")?.Value
                ?? string.Empty);

        private void SalvarToken(string usuarioLogin)
        {
           // cache.Salvar(ObterChaveLogin(usuarioLogin), tokenGerado);
        }

        private bool TokenAtivo(string token)
        {
            //var tokenCache = cache.Obter(ObterChaveToken(token));

            //// Quando não conseguir obter o token do cache assume que o atual é valido
            //if (tokenCache == null)
            //    return true;

            //var tokenAtual = ObterTokenAtual();
            //if (tokenAtual == tokenCache)
            //    return true;

            //// Verifica se o token atual é mais recente que o cache
            //if ((tokenCache != token) && (ObterDataHoraCriacao(tokenAtual) > ObterDataHoraCriacao(tokenCache)))
            //{
            //    tokenGerado = tokenAtual;

            //    SalvarToken(ObterLogin());
            //    return true;
            //}

            return false;
        }

        #endregion Private Methods
    }
}