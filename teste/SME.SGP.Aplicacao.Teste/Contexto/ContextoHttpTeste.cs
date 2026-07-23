using Microsoft.AspNetCore.Http;
using SME.SGP.Infra.Constantes;
using SME.SGP.Infra.Contexto;
using System;
using System.Security.Claims;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Contexto
{
    public class ContextoHttpTeste
    {
        [Fact]
        public void Deve_capturar_dados_de_auditoria_do_contrato_atual_do_token()
        {
            var perfil = Guid.NewGuid().ToString();
            var contextoHttp = CriarContextoHttpComToken(
                new Claim(ClaimTypes.Name, "usuario.teste"),
                new Claim(ClaimsConstants.Login, "usuario.teste"),
                new Claim(ClaimsConstants.Nome, "Usuário Teste"),
                new Claim(ClaimsConstants.Rf, "1234567"),
                new Claim(ClaimsConstants.Perfil, perfil));

            var contexto = new ContextoHttp(new HttpContextAccessor { HttpContext = contextoHttp });

            Assert.Equal("Usuário Teste", contexto.NomeUsuario);
            Assert.Equal("usuario.teste", contexto.UsuarioLogado);
            Assert.Equal("1234567", contexto.ObterVariavel<string>("RF"));
            Assert.Equal("usuario.teste", contexto.ObterVariavel<string>("login"));
            Assert.Equal(perfil, contexto.PerfilUsuario);
        }

        [Theory]
        [InlineData(ClaimsConstants.Rf)]
        [InlineData(ClaimsConstants.Nome)]
        public void Nao_deve_aceitar_token_bearer_sem_claim_obrigatoria_de_auditoria(string claimAusente)
        {
            var contextoHttp = CriarContextoHttpComToken(
                new Claim(ClaimsConstants.Login, "usuario.teste"),
                new Claim(ClaimsConstants.Nome, claimAusente == ClaimsConstants.Nome ? string.Empty : "Usuário Teste"),
                new Claim(ClaimsConstants.Rf, claimAusente == ClaimsConstants.Rf ? string.Empty : "1234567"));

            var accessor = new HttpContextAccessor { HttpContext = contextoHttp };

            var excecao = Assert.Throws<InvalidOperationException>(() => new ContextoHttp(accessor));
            Assert.Contains("claims obrigatórias 'rf' e 'nome'", excecao.Message);
        }

        [Fact]
        public void Deve_preservar_identidade_do_sistema_quando_nao_existe_requisicao_http()
        {
            var contexto = new ContextoHttp(new HttpContextAccessor());

            Assert.Equal("Sistema", contexto.NomeUsuario);
            Assert.Equal("Sistema", contexto.UsuarioLogado);
            Assert.Equal("0", contexto.ObterVariavel<string>("RF"));
        }

        [Fact]
        public void Deve_preservar_identidade_do_sistema_em_requisicao_sem_token_bearer()
        {
            var contexto = new ContextoHttp(new HttpContextAccessor { HttpContext = new DefaultHttpContext() });

            Assert.Equal("Sistema", contexto.NomeUsuario);
            Assert.Equal("0", contexto.ObterVariavel<string>("RF"));
        }

        private static DefaultHttpContext CriarContextoHttpComToken(params Claim[] claims)
        {
            var contextoHttp = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(claims, "Bearer"))
            };
            contextoHttp.Request.Headers["Authorization"] = "Bearer token-para-teste";
            return contextoHttp;
        }
    }
}
