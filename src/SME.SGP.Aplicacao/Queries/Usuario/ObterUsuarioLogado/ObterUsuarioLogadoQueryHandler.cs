using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Contexto;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioLogadoQueryHandler : IRequestHandler<ObterUsuarioLogadoQuery, Usuario>
    {

        private const string CLAIM_PERFIL_ATUAL = "perfil";

        private readonly IContextoAplicacao contextoAplicacao;
        private readonly IRepositorioUsuario repositorioUsuario;
        private readonly IRepositorioCache repositorioCache;
        private readonly IServicoEol servicoEOL;
        private readonly IRepositorioPrioridadePerfil repositorioPrioridadePerfil;
        public ObterUsuarioLogadoQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioUsuario repositorioUsuario, 
            IRepositorioCache repositorioCache, IServicoEol servicoEOL, IRepositorioPrioridadePerfil repositorioPrioridadePerfil)
        {
            this.contextoAplicacao = contextoAplicacao ?? throw new ArgumentNullException(nameof(contextoAplicacao));
            this.repositorioUsuario = repositorioUsuario ?? throw new ArgumentNullException(nameof(repositorioUsuario));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.repositorioPrioridadePerfil = repositorioPrioridadePerfil ?? throw new ArgumentNullException(nameof(repositorioPrioridadePerfil));
        }

        public string ObterLoginAtual()
        {
            var loginAtual = contextoAplicacao.ObterVariavel<string>("login");
            if (loginAtual == null)
                throw new NegocioException("Não foi possível localizar o login no token");

            return loginAtual;
        }

        public async Task<Usuario> Handle(ObterUsuarioLogadoQuery request, CancellationToken cancellationToken)
        {
            var login = ObterLoginAtual();
            if (string.IsNullOrWhiteSpace(login))
                throw new NegocioException("Usuário não encontrado.");

            var usuario = repositorioUsuario.ObterPorCodigoRfLogin(string.Empty, login);

            if (usuario == null)
            {
                throw new NegocioException("Usuário não encontrado.");
            }

            if (!string.IsNullOrEmpty(contextoAplicacao.NomeUsuario))
                usuario.Nome = contextoAplicacao.NomeUsuario;

            var chaveRedis = $"perfis-usuario-{login}";
            var perfisUsuarioString = repositorioCache.Obter(chaveRedis);

            IEnumerable<PrioridadePerfil> perfisDoUsuario = null;

            perfisDoUsuario = string.IsNullOrWhiteSpace(perfisUsuarioString)
                ? await ObterPerfisUsuario(login)
                : JsonConvert.DeserializeObject<IEnumerable<PrioridadePerfil>>(perfisUsuarioString);

            usuario.DefinirPerfis(perfisDoUsuario);
            usuario.DefinirPerfilAtual(ObterPerfilAtual());

            return usuario;

        }
        public string ObterClaim(string nomeClaim)
        {
            var claim = contextoAplicacao.ObterVariavel<IEnumerable<InternalClaim>>("Claims").FirstOrDefault(a => a.Type == nomeClaim);
            return claim?.Value;
        }
        public Guid ObterPerfilAtual()
        {
            return Guid.Parse(ObterClaim(CLAIM_PERFIL_ATUAL));
        }
        public async Task<IEnumerable<PrioridadePerfil>> ObterPerfisUsuario(string login)
        {
            var chaveRedis = $"perfis-usuario-{login}";

            var perfisPorLogin = await servicoEOL.ObterPerfisPorLogin(login);

            if (perfisPorLogin == null)
                throw new NegocioException($"Não foi possível obter os perfis do usuário {login}");

            var perfisDoUsuario = repositorioPrioridadePerfil.ObterPerfisPorIds(perfisPorLogin.Perfis);

            await repositorioCache.SalvarAsync(chaveRedis, JsonConvert.SerializeObject(perfisDoUsuario));

            return perfisDoUsuario;
        }
    }
}
