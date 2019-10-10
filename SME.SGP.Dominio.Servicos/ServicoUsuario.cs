using Microsoft.AspNetCore.Http;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public class ServicoUsuario : IServicoUsuario
    {
        private const string CLAIM_RF = "rf";
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IRepositorioUsuario repositorioUsuario;
        private readonly IServicoEOL servicoEOL;

        public ServicoUsuario(IRepositorioUsuario repositorioUsuario,
                              IServicoEOL servicoEOL,
                              IHttpContextAccessor httpContextAccessor)
        {
            this.repositorioUsuario = repositorioUsuario ?? throw new ArgumentNullException(nameof(repositorioUsuario));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public string ObterClaim(string nomeClaim)
        {
            var claim = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(a => a.Type == nomeClaim);
            return claim?.Value;
        }

        public string ObterLoginAtual()
        {
            var loginAtual = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(a => a.Type == "login");
            if (loginAtual == null)
                throw new NegocioException("Não foi possível localizar o login no token");

            return loginAtual.Value;
        }

        public string ObterRf()
        {
            var rf = ObterClaim(CLAIM_RF);
            return rf;
        }

        public Usuario ObterUsuarioPorCodigoRfLoginOuAdiciona(string codigoRf, string login = "")
        {
            var usuario = repositorioUsuario.ObterPorCodigoRfLogin(codigoRf, login);
            if (usuario != null)
                return usuario;

            usuario = new Usuario() { CodigoRf = codigoRf, Login = login };

            repositorioUsuario.Salvar(usuario);

            return usuario;
        }

        public async Task PodeModificarPerfil(string perfilParaModificar, string login)
        {
            var perfisDoUsuario = await servicoEOL.ObterPerfisPorLogin(login);
            if (perfisDoUsuario == null)
                throw new NegocioException($"Não foi possível obter os perfis do usuário {login}");

            if (!perfisDoUsuario.Perfis.Contains(Guid.Parse(perfilParaModificar)))
                throw new NegocioException($"O usuário {login} não possui acesso ao perfil {perfilParaModificar}");
        }
    }
}