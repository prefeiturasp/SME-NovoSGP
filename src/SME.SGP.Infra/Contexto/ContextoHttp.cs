using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SME.SGP.Infra.Contexto
{
    public class ContextoHttp : ContextoBase
    {
        readonly IHttpContextAccessor httpContextAccessor;

        public ContextoHttp(IHttpContextAccessor httpContextAccessor)
            : base()
        {
            this.httpContextAccessor = httpContextAccessor;

            CapturarVariaveis();
        }

        private void CapturarVariaveis()
        {
            var contextoHttp = httpContextAccessor.HttpContext;
            var usuario = contextoHttp?.User;
            var authorizationHeader = contextoHttp?.Request?.Headers["authorization"];
            var temAuthorizationHeader = authorizationHeader.HasValue && authorizationHeader.Value != StringValues.Empty;
            var codigoRf = usuario?.FindFirst(ClaimsConstants.Rf)?.Value;
            var nomeUsuario = usuario?.FindFirst(ClaimsConstants.Nome)?.Value;

            var temTokenBearer = temAuthorizationHeader && authorizationHeader.Value.Any(valor =>
                valor.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase));

            ValidarClaimsAuditoria(temTokenBearer, codigoRf, nomeUsuario);

            Variaveis.Add("RF", codigoRf ?? "0");
            Variaveis.Add("Claims", GetInternalClaim());
            Variaveis.Add("login", usuario?.FindFirst(ClaimsConstants.Login)?.Value ?? string.Empty);
            Variaveis.Add("NumeroPagina", contextoHttp?.Request?.Query["NumeroPagina"].FirstOrDefault() ?? "0");
            Variaveis.Add("NumeroRegistros", contextoHttp?.Request?.Query["NumeroRegistros"].FirstOrDefault() ?? "0");

            Variaveis.Add("UsuarioLogado", usuario?.Identity?.Name ?? "Sistema");
            Variaveis.Add("NomeUsuario", nomeUsuario ?? "Sistema");
            Variaveis.Add("Administrador", usuario?.FindFirst("login_adm_suporte")?.Value ?? string.Empty);
            Variaveis.Add("NomeAdministrador", usuario?.FindFirst("nome_adm_suporte")?.Value ?? string.Empty);
            Variaveis.Add("PerfilUsuario", ObterPerfilAtual());

            if (!temAuthorizationHeader)
            {
                Variaveis.Add("TemAuthorizationHeader", false);
                Variaveis.Add("TokenAtual", string.Empty);
            }
            else
            {
                Variaveis.Add("TemAuthorizationHeader", true);
                Variaveis.Add("TokenAtual", authorizationHeader.Value.Single().Split(' ').Last());
            }
        }

        private static void ValidarClaimsAuditoria(bool temTokenBearer, string codigoRf, string nomeUsuario)
        {
            if (temTokenBearer && (string.IsNullOrWhiteSpace(codigoRf) || string.IsNullOrWhiteSpace(nomeUsuario)))
                throw new InvalidOperationException("O token autenticado não contém as claims obrigatórias 'rf' e 'nome' para auditoria.");
        }

        private IEnumerable<InternalClaim> GetInternalClaim()
        {
            return (httpContextAccessor.HttpContext?.User?.Claims ?? Enumerable.Empty<Claim>()).Select(x => new InternalClaim() { Type = x.Type, Value = x.Value }).ToList();
        }

        private string ObterPerfilAtual()
        {
            return httpContextAccessor.HttpContext?.User?.FindFirst(ClaimsConstants.Perfil)?.Value;
        }

        public override IContextoAplicacao AtribuirContexto(IContextoAplicacao contexto)
        {
            throw new NotImplementedException("Este tipo de conexto não permite atribuição");
        }

        public override void AdicionarVariaveis(IDictionary<string, object> variaveis)
        {
            this.Variaveis = variaveis;
        }
    }
}
