using Newtonsoft.Json;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Contexto;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public class ServicoUsuario : IServicoUsuario
    {
        private const string CLAIM_PERFIL_ATUAL = "perfil";
        private const string CLAIM_PERMISSAO = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
        private const string CLAIM_RF = "rf";
        private readonly IContextoAplicacao contextoAplicacao;
        private readonly IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ;
        private readonly IRepositorioCache repositorioCache;
        private readonly IRepositorioPrioridadePerfil repositorioPrioridadePerfil;
        private readonly IRepositorioUsuario repositorioUsuario;
        private readonly IServicoEOL servicoEOL;
        private readonly IUnitOfWork unitOfWork;

        public ServicoUsuario(IRepositorioUsuario repositorioUsuario,
                                      IServicoEOL servicoEOL,
                              IRepositorioPrioridadePerfil repositorioPrioridadePerfil,
                              IUnitOfWork unitOfWork,
                              IContextoAplicacao contextoAplicacao,
                              IRepositorioCache repositorioCache,
                              IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ)
        {
            this.repositorioUsuario = repositorioUsuario ?? throw new ArgumentNullException(nameof(repositorioUsuario));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.repositorioPrioridadePerfil = repositorioPrioridadePerfil;
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.contextoAplicacao = contextoAplicacao ?? throw new ArgumentNullException(nameof(contextoAplicacao));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ?? throw new ArgumentNullException(nameof(repositorioAtribuicaoCJ));
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
            var claim = contextoAplicacao.ObterVarivel<IEnumerable<InternalClaim>>("Claims").FirstOrDefault(a => a.Type == nomeClaim);
            return claim?.Value;
        }

        public string ObterLoginAtual()
        {
            var loginAtual = contextoAplicacao.ObterVarivel<string>("login");
            if (loginAtual == null)
                throw new NegocioException("Não foi possível localizar o login no token");

            return loginAtual;
        }

        public string ObterNomeLoginAtual()
        {
            var nomeLoginAtual = contextoAplicacao.ObterVarivel<string>("NomeUsuario");
            if (nomeLoginAtual == null)
                throw new NegocioException("Não foi possível localizar o nome do login no token");

            return nomeLoginAtual;
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

            _ = repositorioCache.SalvarAsync(chaveRedis, JsonConvert.SerializeObject(perfisDoUsuario));

            return perfisDoUsuario;
        }

        public IEnumerable<Permissao> ObterPermissoes()
        {
            var claims = contextoAplicacao.ObterVarivel<IEnumerable<InternalClaim>>("Claims").Where(a => a.Type == CLAIM_PERMISSAO);
            List<Permissao> retorno = new List<Permissao>();

            if (claims.Any())
            {
                foreach (var claim in claims)
                {
                    var permissao = (Permissao)Enum.Parse(typeof(Permissao), claim.Value);
                    retorno.Add(permissao);
                }
            }
            return retorno;
        }

        public string ObterRf()
        {
            var rf = ObterClaim(CLAIM_RF);
            return rf;
        }

        public async Task<Usuario> ObterUsuarioLogado()
        {
            var login = ObterLoginAtual();
            var usuario = repositorioUsuario.ObterPorCodigoRfLogin(string.Empty, login);

            if (usuario == null)
            {
                throw new NegocioException("Usuário não encontrado.");
            }

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

        public Usuario ObterUsuarioPorCodigoRfLoginOuAdiciona(string codigoRf, string login = "", string nome = "", string email = "")
        {
            var eNumero = int.TryParse(codigoRf, out int n);

            codigoRf = eNumero ? codigoRf : null;

            var usuario = repositorioUsuario.ObterPorCodigoRfLogin(codigoRf, login);
            if (usuario != null)
            {
                if (string.IsNullOrEmpty(usuario.Nome) && !string.IsNullOrEmpty(nome))
                {
                    usuario.Nome = nome;
                    repositorioUsuario.Salvar(usuario);
                }

                if (string.IsNullOrEmpty(usuario.CodigoRf) && !string.IsNullOrEmpty(codigoRf))
                {
                    usuario.CodigoRf = codigoRf;
                    repositorioUsuario.Salvar(usuario);
                }

                return usuario;
            }

            if (string.IsNullOrEmpty(login))
                login = codigoRf;
                      

            usuario = new Usuario() { CodigoRf = codigoRf, Login = login, Nome = nome };

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

        public async Task<bool> PodePersistirTurma(string codigoRf, string turmaId, DateTime data)
        {
            var usuarioLogado = await ObterUsuarioLogado();

            if (!usuarioLogado.EhProfessorCj())
                return await servicoEOL.ProfessorPodePersistirTurma(codigoRf, turmaId, data);

            var atribuicaoCj = repositorioAtribuicaoCJ.ObterAtribuicaoAtiva(codigoRf);

            return atribuicaoCj != null && atribuicaoCj.Any();
        }

        public async Task<bool> PodePersistirTurmaNasDatas(string codigoRf, string turmaId, string disciplinaId, DateTime data, Usuario usuario = null)
        {
            if (usuario == null)
                usuario = repositorioUsuario.ObterPorCodigoRfLogin(codigoRf, string.Empty);

            if (!usuario.EhProfessorCj())
            {
                var validacaoData = await servicoEOL.PodePersistirTurmaNasDatas(usuario.CodigoRf, turmaId, new string[] { data.ToString("s") }, long.Parse(disciplinaId));

                if (validacaoData == null || !validacaoData.Any())
                    throw new NegocioException("Não foi possível obter a validação do professor no EOL.");

                return validacaoData.FirstOrDefault().PodePersistir;
            }
            var atribuicaoCj = repositorioAtribuicaoCJ.ObterAtribuicaoAtiva(usuario.CodigoRf);

            return atribuicaoCj != null && atribuicaoCj.Any();
        }

        public async Task<bool> PodePersistirTurmaDisciplina(string codigoRf, string turmaId, string disciplinaId, DateTime data, Usuario usuario = null)
        {
            if (usuario == null)
                usuario = await ObterUsuarioLogado();

            if (!usuario.EhProfessorCj())
                return await servicoEOL.PodePersistirTurmaDisciplina(usuario.CodigoRf, turmaId, disciplinaId, data);
            
            var atribuicaoCj = repositorioAtribuicaoCJ.ObterAtribuicaoAtiva(usuario.CodigoRf);

            return atribuicaoCj != null && atribuicaoCj.Any();
        }

        public void RemoverPerfisUsuarioAtual()
        {
            var login = ObterLoginAtual();

            RemoverPerfisUsuarioCache(login);
        }

        public void RemoverPerfisUsuarioCache(string login)
        {
            var chaveRedis = $"perfis-usuario-{login}";

            _ = repositorioCache.RemoverAsync(chaveRedis);
        }

        public bool UsuarioLogadoPossuiPerfilSme()
        {
            var usuarioLogado = ObterUsuarioLogado().Result;

            return usuarioLogado.PossuiPerfilSme();
        }

        private async Task AlterarEmail(Usuario usuario, string novoEmail)
        {
            var outrosUsuariosComMesmoEmail = await servicoEOL.ExisteUsuarioComMesmoEmail(usuario.Login, novoEmail);

            if (outrosUsuariosComMesmoEmail)
                throw new NegocioException("Já existe outro usuário com o e-mail informado.");

            var retornoEol = await servicoEOL.ObterPerfisPorLogin(usuario.Login);
            if (retornoEol == null || retornoEol.Status != AutenticacaoStatusEol.Ok)
                throw new NegocioException("Ocorreu um erro ao obter os dados do usuário no EOL.");

            if (retornoEol.Perfis == null || !retornoEol.Perfis.Any())
            {
                //pode ser que esse usuário não tenha se logado ainda no sistema, realizar chamada para o serviço de relacionar grupos
                retornoEol = await servicoEOL.RelecionarUsuarioPerfis(usuario.Login);
                if (retornoEol == null || !retornoEol.Perfis.Any())
                    throw new NegocioException("Não é possível alterar o e-mail deste usuário pois o mesmo está sem perfis de acesso.");
            }
            var perfisUsuario = repositorioPrioridadePerfil.ObterPerfisPorIds(retornoEol.Perfis);
            usuario.DefinirPerfis(perfisUsuario);
            usuario.DefinirEmail(novoEmail);
            repositorioUsuario.Salvar(usuario);
            await servicoEOL.AlterarEmail(usuario.Login, novoEmail);
        }
    }
}