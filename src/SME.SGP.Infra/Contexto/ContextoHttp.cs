using Microsoft.AspNetCore.Http;
using SME.SGP.Dominio;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

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
            Variaveis.Add("RF", httpContextAccessor.HttpContext?.User?.FindFirst("RF")?.Value ?? "0");
            Variaveis.Add("Claims", GetInternalClaim());
            Variaveis.Add("login", httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(a => a.Type == "login")?.Value ?? string.Empty);
            Variaveis.Add("NumeroPagina", httpContextAccessor.HttpContext?.Request?.Query["NumeroPagina"].FirstOrDefault() ?? "0");
            Variaveis.Add("NumeroRegistros", httpContextAccessor.HttpContext?.Request?.Query["NumeroRegistros"].FirstOrDefault() ?? "0");

            Variaveis.Add("UsuarioLogado", httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Sistema");
            Variaveis.Add("NomeUsuario", httpContextAccessor.HttpContext?.User?.FindFirst("Nome")?.Value ?? "Sistema");
        }

        private IEnumerable<InternalClaim> GetInternalClaim()
        {
            return (httpContextAccessor.HttpContext?.User?.Claims ?? Enumerable.Empty<Claim>()).Select(x => new InternalClaim() { Type = x.Type, Value = x.Value }).ToList();
        }

        public override IContextoAplicacao AtribuirContexto(IContextoAplicacao contexto)
        {
            throw new Exception("Este tipo de conexto não permite atribuição");
        }
    }
}
