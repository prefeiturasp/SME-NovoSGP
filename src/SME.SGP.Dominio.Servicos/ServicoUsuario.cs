using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Constantes;
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
        private readonly IMediator mediator;
        private readonly IRepositorioCache repositorioCache;
        private readonly IRepositorioPrioridadePerfil repositorioPrioridadePerfil;
        private readonly IRepositorioUsuario repositorioUsuario;
        private readonly IUnitOfWork unitOfWork;

        private Usuario usuarioLogado { get; set; }

        public ServicoUsuario(IRepositorioUsuario repositorioUsuario,
                              IRepositorioPrioridadePerfil repositorioPrioridadePerfil,
                              IUnitOfWork unitOfWork,
                              IContextoAplicacao contextoAplicacao,
                              IRepositorioCache repositorioCache,
                              IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ,
                              IMediator mediator)
        {
            this.repositorioUsuario = repositorioUsuario ?? throw new ArgumentNullException(nameof(repositorioUsuario));
            this.repositorioPrioridadePerfil = repositorioPrioridadePerfil;
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.contextoAplicacao = contextoAplicacao ?? throw new ArgumentNullException(nameof(contextoAplicacao));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ?? throw new ArgumentNullException(nameof(repositorioAtribuicaoCJ));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task AlterarEmailUsuarioPorLogin(string login, string novoEmail)
        {
            var usuario = await mediator.Send(new ObterUsuarioPorCodigoRfLoginQuery(string.Empty, login));

            if (usuario.EhNulo())
                throw new NegocioException("Usuário não encontrado.");

            await AlterarEmail(usuario, novoEmail);
        }
        public async Task<Usuario> ObterPorIdAsync(long id)
        {
            return await repositorioUsuario.ObterPorIdAsync(id);
        }

        public async Task AlterarEmailUsuarioPorRfOuInclui(string codigoRf, string novoEmail)
        {
            unitOfWork.IniciarTransacao();
            try
            {
                var usuario = await ObterUsuarioPorCodigoRfLoginOuAdiciona(codigoRf);
                await AlterarEmail(usuario, novoEmail);

                unitOfWork.PersistirTransacao();
            } catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        public string ObterClaim(string nomeClaim)
        {
            var claim = contextoAplicacao.ObterVariavel<IEnumerable<InternalClaim>>("Claims").FirstOrDefault(a => a.Type == nomeClaim);
            return claim?.Value;
        }

        public string ObterLoginAtual()
        {
            var loginAtual = contextoAplicacao.ObterVariavel<string>("login");

            if (loginAtual.EhNulo())
                throw new NegocioException("Não foi possível localizar o login no token");

            return loginAtual;
        }

        public string ObterNomeLoginAtual()
        {
            var nomeLoginAtual = contextoAplicacao.ObterVariavel<string>("NomeUsuario");
            if (nomeLoginAtual.EhNulo())
                throw new NegocioException("Não foi possível localizar o nome do login no token");

            return nomeLoginAtual;
        }

        public Guid ObterPerfilAtual()
        {
            return Guid.Parse(ObterClaim(CLAIM_PERFIL_ATUAL));
        }

        public async Task<IEnumerable<PrioridadePerfil>> ObterPerfisUsuario(string login)
        {
            var chaveCache = string.Format(NomeChaveCache.PERFIS_USUARIO, login);

            var perfisPorLogin = await mediator.Send(new ObterPerfisPorLoginQuery(login));

            if (perfisPorLogin.EhNulo())
                throw new NegocioException($"Não foi possível obter os perfis do usuário {login}");

            var perfisDoUsuario = repositorioPrioridadePerfil.ObterPerfisPorIds(perfisPorLogin.Perfis);

            _ = repositorioCache.SalvarAsync(chaveCache, JsonConvert.SerializeObject(perfisDoUsuario));

            return perfisDoUsuario;
        }

        public IEnumerable<Permissao> ObterPermissoes()
        {
            var claims = contextoAplicacao.ObterVariavel<IEnumerable<InternalClaim>>("Claims").Where(a => a.Type == CLAIM_PERMISSAO);
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
            if (usuarioLogado.EhNulo())
                usuarioLogado = await IdentificaUsuarioLogado();

            return usuarioLogado;
        }

        private async Task<Usuario> IdentificaUsuarioLogado()
        {
            var login = ObterLoginAtual();
            if (string.IsNullOrEmpty(login))
                return null;

            var usuario = await mediator.Send(new ObterUsuarioPorCodigoRfLoginQuery(string.Empty, login));

            if (usuario.EhNulo())
                throw new NegocioException("Usuário não encontrado.");

            if (!string.IsNullOrEmpty(contextoAplicacao.NomeUsuario))
                usuario.Nome = contextoAplicacao.NomeUsuario;

            if (usuario.Perfis.NaoEhNulo() && usuario.Perfis.Any())
                return usuario;

            var chaveCache = string.Format(NomeChaveCache.PERFIS_USUARIO, login);
            var perfisDoUsuario = await repositorioCache.ObterAsync(chaveCache, async () => await ObterPerfisUsuario(login));

            usuario.DefinirPerfis(perfisDoUsuario);
            usuario.DefinirPerfilAtual(ObterPerfilAtual());

            return usuario;
        }

        public async Task<Usuario> ObterUsuarioPorCodigoRfLoginOuAdiciona(string codigoRf, string login = "", string nome = "", string email = "", bool buscaLogin = false)
        {
            var eNumero = long.TryParse(codigoRf, out long n);
            var ehCpf = codigoRf.SomenteNumeros().Length == 11;
            
            codigoRf = eNumero || (!eNumero && ehCpf) ? codigoRf.SomenteNumeros() : null;
            var usuario = await mediator.Send(new ObterUsuarioPorCodigoRfLoginQuery(buscaLogin ? null : codigoRf, login));

            if (usuario.NaoEhNulo())
            {
                var atualizouNome = AtualizouNomeDoUsuario(usuario, nome);
                var atualizouRF = AtualizouRfDoUsuario(usuario, codigoRf);

                usuario.Nome = usuario?.Nome ?? "";

                if (atualizouNome || atualizouRF)
                    await repositorioUsuario.SalvarAsync(usuario);

                return usuario;
            }

            if (string.IsNullOrEmpty(login))
                login = codigoRf;

            usuario = new Usuario() { CodigoRf = codigoRf, Login = login, Nome = nome };

            await repositorioUsuario.SalvarAsync(usuario);

            return usuario;
        }

        public async Task PodeModificarPerfil(Guid perfilParaModificar, string login)
        {
            var perfisDoUsuario = await mediator.Send(new ObterPerfisPorLoginQuery(login));
            if (perfisDoUsuario.EhNulo())
                throw new NegocioException($"Não foi possível obter os perfis do usuário {login}");

            if (!perfisDoUsuario.Perfis.Contains(perfilParaModificar))
                throw new NegocioException($"O usuário {login} não possui acesso ao perfil {perfilParaModificar}");
        }

        public async Task<bool> PodePersistirTurma(string codigoRf, string turmaId, DateTime data)
        {
            var usuarioLogado = await ObterUsuarioLogado();

            if (!usuarioLogado.EhProfessorCj())
                return await mediator.Send(new ProfessorPodePersistirTurmaQuery(codigoRf, turmaId, data));

            var atribuicaoCj = repositorioAtribuicaoCJ
                .ObterAtribuicaoAtiva(codigoRf, false);

            return atribuicaoCj.NaoEhNulo() && atribuicaoCj.Any();
        }

        public async Task<bool> PodePersistirTurmaNasDatas(string codigoRf, string turmaId, string disciplinaId, DateTime data, Usuario usuario = null)
        {
            if (usuario.EhNulo())
                usuario = await mediator.Send(new ObterUsuarioPorCodigoRfLoginQuery(codigoRf, string.Empty));

            if (!usuario.EhProfessorCj())
            {
                var validacaoData = await mediator.Send(new ObterValidacaoPodePersistirTurmaNasDatasQuery(usuario.CodigoRf, turmaId, new DateTime[] { data }, long.Parse(disciplinaId)));

                if (validacaoData.EhNulo() || !validacaoData.Any())
                    throw new NegocioException("Não foi possível obter a validação do professor no EOL.");

                return validacaoData.FirstOrDefault().PodePersistir;
            }

            var atribuicaoCj = repositorioAtribuicaoCJ
                .ObterAtribuicaoAtiva(usuario.CodigoRf, false);

            return atribuicaoCj.NaoEhNulo() && atribuicaoCj.Any();
        }

        public async Task<bool> PodePersistirTurmaDisciplina(string codigoRf, string turmaId, string disciplinaId, DateTime data, Usuario usuario = null)
        {
            if (usuario.EhNulo())
                usuario = await ObterUsuarioLogado();

            if (!usuario.EhProfessorCj())
                return await mediator.Send(new ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery(Int64.Parse(disciplinaId), turmaId, data, usuario));

            var atribuicaoCj = repositorioAtribuicaoCJ
                .ObterAtribuicaoAtiva(usuario.CodigoRf, false);

            return atribuicaoCj.NaoEhNulo() && atribuicaoCj.Any();
        }

        public void RemoverPerfisUsuarioAtual()
        {
            var login = ObterLoginAtual();

            RemoverPerfisUsuarioCache(login);
        }

        public void RemoverPerfisUsuarioCache(string login)
        {
            var chaveCache = string.Format(NomeChaveCache.PERFIS_USUARIO, login);

            _ = repositorioCache.RemoverAsync(chaveCache);
        }

        public bool UsuarioLogadoPossuiPerfilSme()
        {
            var usuarioLogado = ObterUsuarioLogado().Result;

            return usuarioLogado.PossuiPerfilSme();
        }

        private async Task AlterarEmail(Usuario usuario, string novoEmail)
        {
            var outrosUsuariosComMesmoEmail = await mediator.Send(new ExisteUsuarioComMesmoEmailQuery(usuario.Login, novoEmail));

            if (outrosUsuariosComMesmoEmail)
                throw new NegocioException("Já existe outro usuário com o e-mail informado.");

            var retornoEol = await mediator.Send(new ObterPerfisPorLoginQuery(usuario.Login));
            if (retornoEol.EhNulo())
                throw new NegocioException("Ocorreu um erro ao obter os dados/perfis do usuário no EOL.");


            var perfisUsuario = repositorioPrioridadePerfil.ObterPerfisPorIds(retornoEol.Perfis);
            usuario.DefinirPerfis(perfisUsuario);
            usuario.DefinirEmail(novoEmail);
            repositorioUsuario.Salvar(usuario);
            await mediator.Send(new AlterarEmailUsuarioCommand(usuario.Login, novoEmail));
        }

        public async Task<string[]> ObterComponentesCurricularesQuePodeVisualizarHoje(string turmaCodigo, Usuario usuarioLogado)
        {
            var componentesCurricularesParaVisualizar = new List<string>();

            var componentesCurricularesUsuarioLogado = await mediator.Send(new ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery(turmaCodigo, usuarioLogado.CodigoRf, usuarioLogado.PerfilAtual));

            var componentesCurricularesIdsUsuarioLogado = componentesCurricularesUsuarioLogado.Select(b => b.Codigo.ToString());
            var hoje = DateTime.Today;

            foreach (var componenteParaVerificarAtribuicao in componentesCurricularesIdsUsuarioLogado)
            {
                if (await mediator.Send(new PodePersistirTurmaDisciplinaQuery(usuarioLogado.CodigoRf, turmaCodigo, componenteParaVerificarAtribuicao, hoje.Ticks)))
                    componentesCurricularesParaVisualizar.Add(componenteParaVerificarAtribuicao);
            }

            return componentesCurricularesParaVisualizar.ToArray();
        }

        private bool AtualizouNomeDoUsuario(Usuario usuario, string nome)
        {
            if (!string.IsNullOrEmpty(nome))
            {
                if (string.IsNullOrEmpty(usuario.Nome))
                {
                    usuario.Nome = nome;
                    return true;
                }
                else if (!usuario.Nome.Equals(nome))
                {
                    usuario.Nome = nome;
                    return true;
                }
            }

            return false;
        }

        private bool AtualizouRfDoUsuario(Usuario usuario, string codigoRf)
        {
            if (string.IsNullOrEmpty(usuario.CodigoRf) && !string.IsNullOrEmpty(codigoRf))
            {
                usuario.CodigoRf = codigoRf;

                return true;
            }

            return false;
        }
    }
}