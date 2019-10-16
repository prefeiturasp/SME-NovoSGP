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
        private const string CLAIM_PERFIL = "perfil";
        private const string CLAIM_RF = "rf";
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IRepositorioPrioridadePerfil repositorioPrioridadePerfil;
        private readonly IRepositorioUsuario repositorioUsuario;
        private readonly IServicoEOL servicoEOL;
        private readonly IUnitOfWork unitOfWork;

        public ServicoUsuario(IRepositorioUsuario repositorioUsuario,
                              IServicoEOL servicoEOL,
                              IRepositorioPrioridadePerfil repositorioPrioridadePerfil,
                              IUnitOfWork unitOfWork,
                              IHttpContextAccessor httpContextAccessor)
        {
            this.repositorioUsuario = repositorioUsuario ?? throw new ArgumentNullException(nameof(repositorioUsuario));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.repositorioPrioridadePerfil = repositorioPrioridadePerfil;
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task AlterarEmailUsuarioPorLogin(string login, string novoEmail)
        {
            var usuario = repositorioUsuario.ObterPorCodigoRfLogin(string.Empty, login);
            if (usuario == null)
                throw new NegocioException("Usuário não encontrado.");

            await AlterarEmail(usuario, novoEmail);
        }

        public async Task AlterarEmailUsuarioPorRfOuInclui(string codigoRf, string novoEmail)
        {
            unitOfWork.IniciarTransacao();

            var usuario = ObterUsuarioPorCodigoRfLoginOuAdiciona(codigoRf);
            await AlterarEmail(usuario, novoEmail);

            unitOfWork.PersistirTransacao();
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

        public Guid ObterPerfilAtual()
        {
            return Guid.Parse(ObterClaim(CLAIM_PERFIL));
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

            if (string.IsNullOrEmpty(login))
                login = codigoRf;

            usuario = new Usuario() { CodigoRf = codigoRf, Login = login };

            repositorioUsuario.Salvar(usuario);

            return usuario;
        }

        public async Task PodeModificarPerfil(Guid perfilParaModificar, string login)
        {
            var perfisDoUsuario = await servicoEOL.ObterPerfisPorLogin(login);
            if (perfisDoUsuario == null)
                throw new NegocioException($"Não foi possível obter os perfis do usuário {login}");

            if (!perfisDoUsuario.Perfis.Contains(perfilParaModificar))
                throw new NegocioException($"O usuário {login} não possui acesso ao perfil {perfilParaModificar}");
        }

        private async Task AlterarEmail(Usuario usuario, string novoEmail)
        {
            var outrosUsuariosComMesmoEmail = repositorioUsuario.ExisteUsuarioComMesmoEmail(novoEmail, usuario.Id);

            if (outrosUsuariosComMesmoEmail)
                throw new NegocioException("Já existe outro usuário com o e-mail informado.");

            var retornoEol = await servicoEOL.ObterPerfisPorLogin(usuario.Login);
            if (retornoEol == null || retornoEol.Status != Dto.AutenticacaoStatusEol.Ok)
                throw new NegocioException("Ocorreu um erro ao obter os dados do usuário no EOL.");

            var perfisUsuario = repositorioPrioridadePerfil.ObterPerfisPorIds(retornoEol.Perfis);
            usuario.DefinirEmail(novoEmail, perfisUsuario);
            repositorioUsuario.Salvar(usuario);
        }
    }
}