using Microsoft.Extensions.Configuration;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosUsuario : IComandosUsuario
    {
        private readonly IConfiguration configuration;
        private readonly IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ;
        private readonly IRepositorioAtribuicaoEsporadica repositorioAtribuicaoEsporadica;
        private readonly IRepositorioCache repositorioCache;
        private readonly IRepositorioUsuario repositorioUsuario;
        private readonly IRepositorioHistoricoEmailUsuario repositorioHistoricoEmailUsuario;
        private readonly IServicoAbrangencia servicoAbrangencia;
        private readonly IServicoAutenticacao servicoAutenticacao;
        private readonly IServicoEmail servicoEmail;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoPerfil servicoPerfil;
        private readonly IServicoTokenJwt servicoTokenJwt;
        private readonly IServicoUsuario servicoUsuario;

        public ComandosUsuario(IRepositorioUsuario repositorioUsuario,
            IServicoAutenticacao servicoAutenticacao,
            IServicoUsuario servicoUsuario,
            IServicoPerfil servicoPerfil,
            IServicoEOL servicoEOL,
            IServicoTokenJwt servicoTokenJwt,
            IServicoEmail servicoEmail,
            IConfiguration configuration,
            IRepositorioCache repositorioCache,
            IServicoAbrangencia servicoAbrangencia,
            IRepositorioAtribuicaoEsporadica repositorioAtribuicaoEsporadica,
            IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ,
            IRepositorioHistoricoEmailUsuario repositorioHistoricoEmailUsuario)
        {
            this.repositorioUsuario = repositorioUsuario ?? throw new ArgumentNullException(nameof(repositorioUsuario));
            this.servicoAutenticacao = servicoAutenticacao ?? throw new ArgumentNullException(nameof(servicoAutenticacao));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.servicoPerfil = servicoPerfil ?? throw new ArgumentNullException(nameof(servicoPerfil));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.servicoTokenJwt = servicoTokenJwt ?? throw new ArgumentNullException(nameof(servicoTokenJwt));
            this.servicoAbrangencia = servicoAbrangencia ?? throw new ArgumentNullException(nameof(servicoAbrangencia));
            this.repositorioAtribuicaoEsporadica = repositorioAtribuicaoEsporadica ?? throw new ArgumentNullException(nameof(repositorioAtribuicaoEsporadica));
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ?? throw new ArgumentNullException(nameof(repositorioAtribuicaoCJ));
            this.repositorioHistoricoEmailUsuario = repositorioHistoricoEmailUsuario ?? throw new ArgumentNullException(nameof(repositorioHistoricoEmailUsuario));
            this.servicoEmail = servicoEmail ?? throw new ArgumentNullException(nameof(servicoEmail));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        //  TODO: aplicar validações permissão de acesso
        public async Task AlterarEmail(AlterarEmailDto alterarEmailDto, string codigoRf)
        {
            await servicoUsuario.AlterarEmailUsuarioPorRfOuInclui(codigoRf, alterarEmailDto.NovoEmail);
            AdicionarHistoricoEmailUsuario(null, codigoRf, alterarEmailDto.NovoEmail, AcaoHistoricoEmailUsuario.ReiniciarSenha);
        }

        public async Task AlterarEmailUsuarioLogado(string novoEmail)
        {
            var login = servicoUsuario.ObterLoginAtual();
            await servicoUsuario.AlterarEmailUsuarioPorLogin(login, novoEmail);
            AdicionarHistoricoEmailUsuario(login, null, novoEmail, AcaoHistoricoEmailUsuario.AlterarEmail);
        }

        public async Task AlterarSenha(AlterarSenhaDto alterarSenhaDto)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(null, login);
            if (usuario == null)
            {
                throw new NegocioException("Usuário não encontrado.");
            }
            usuario.ValidarSenha(alterarSenhaDto.NovaSenha);
            await servicoAutenticacao.AlterarSenha(login, alterarSenhaDto.SenhaAtual, alterarSenhaDto.NovaSenha);
        }

        public async Task AlterarSenhaComTokenRecuperacao(RecuperacaoSenhaDto recuperacaoSenhaDto)
        {
            Usuario usuario = repositorioUsuario.ObterPorTokenRecuperacaoSenha(recuperacaoSenhaDto.Token);
            if (usuario == null)
            {
                throw new NegocioException("Usuário não encontrado.");
            }

            if (!usuario.TokenRecuperacaoSenhaEstaValido())
            {
                throw new NegocioException("Este link expirou. Clique em continuar para solicitar um novo link de recuperação de senha.", 403);
            }

            usuario.ValidarSenha(recuperacaoSenhaDto.NovaSenha);

            var retornoApi = await servicoEOL.AlterarSenha(usuario.Login, recuperacaoSenhaDto.NovaSenha);

            if (!retornoApi.SenhaAlterada)
            {
                throw new NegocioException(retornoApi.Mensagem, retornoApi.StatusRetorno);
            }

            usuario.FinalizarRecuperacaoSenha();
            repositorioUsuario.Salvar(usuario);
        }

        public async Task<AlterarSenhaRespostaDto> AlterarSenhaPrimeiroAcesso(PrimeiroAcessoDto primeiroAcessoDto)
        {
            var usuario = new Usuario();

            usuario.Login = primeiroAcessoDto.Usuario;

            usuario.ValidarSenha(primeiroAcessoDto.NovaSenha);

            return await servicoEOL.AlterarSenha(usuario.Login, primeiroAcessoDto.NovaSenha);
        }

        public async Task<UsuarioAutenticacaoRetornoDto> Autenticar(string login, string senha)
        {
            login = login.Trim().ToLower();

            var retornoAutenticacaoEol = await servicoAutenticacao.AutenticarNoEol(login, senha);

            if (!retornoAutenticacaoEol.Item1.Autenticado)
                return retornoAutenticacaoEol.Item1;

            if (!retornoAutenticacaoEol.Item4 && retornoAutenticacaoEol.Item5)
                retornoAutenticacaoEol.Item3 = ValidarPerfilCJ(retornoAutenticacaoEol.Item2, retornoAutenticacaoEol.Item1.UsuarioId, retornoAutenticacaoEol.Item3, login).Result;

            var dadosUsuario = await servicoEOL.ObterMeusDados(login);

            var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(retornoAutenticacaoEol.Item2, login, dadosUsuario.Nome, dadosUsuario.Email);

            retornoAutenticacaoEol.Item1.PerfisUsuario = await servicoPerfil.DefinirPerfilPrioritario(retornoAutenticacaoEol.Item3, usuario);

            var perfilSelecionado = retornoAutenticacaoEol.Item1.PerfisUsuario.PerfilSelecionado;

            var permissionamentos = await servicoEOL.ObterPermissoesPorPerfil(perfilSelecionado);

            if (permissionamentos == null || !permissionamentos.Any())
            {
                retornoAutenticacaoEol.Item1.Autenticado = false;
                return retornoAutenticacaoEol.Item1;
            }

            var listaPermissoes = permissionamentos
                .Distinct()
                .Select(a => (Permissao)a)
                .ToList();

            // Revoga token atual para geração de um novo
            await servicoTokenJwt.RevogarToken(login);

            // Gera novo token e guarda em cache
            retornoAutenticacaoEol.Item1.Token =
                servicoTokenJwt.GerarToken(login, dadosUsuario.Nome, usuario.CodigoRf, retornoAutenticacaoEol.Item1.PerfisUsuario.PerfilSelecionado, listaPermissoes);

            retornoAutenticacaoEol.Item1.DataHoraExpiracao = servicoTokenJwt.ObterDataHoraExpiracao();
            //var fromDate = servicoTokenJwt.ObterDataHoraCriacao();

            usuario.AtualizaUltimoLogin();

            repositorioUsuario.Salvar(usuario);

            await servicoAbrangencia.Salvar(login, perfilSelecionado, true);

            return retornoAutenticacaoEol.Item1;
        }

        public async Task<TrocaPerfilDto> ModificarPerfil(Guid perfil)
        {
            string loginAtual = servicoUsuario.ObterLoginAtual();
            string codigoRfAtual = servicoUsuario.ObterRf();
            string nomeLoginAtual = servicoUsuario.ObterNomeLoginAtual();

            await servicoUsuario.PodeModificarPerfil(perfil, loginAtual);

            var permissionamentos = await servicoEOL.ObterPermissoesPorPerfil(perfil);

            if (permissionamentos == null || !permissionamentos.Any())
            {
                throw new NegocioException($"Não foi possível obter os permissionamentos do perfil selecionado");
            }
            else
            {
                var listaPermissoes = permissionamentos
                    .Distinct()
                    .Select(a => (Permissao)a)
                    .ToList();

                await servicoAbrangencia.Salvar(loginAtual, perfil, false);
                var usuario = await servicoUsuario.ObterUsuarioLogado();

                usuario.DefinirPerfilAtual(perfil);

                await servicoTokenJwt.RevogarToken(loginAtual);
                var tokenStr = servicoTokenJwt.GerarToken(loginAtual, nomeLoginAtual, codigoRfAtual, perfil, listaPermissoes);

                return new TrocaPerfilDto
                {
                    Token = tokenStr,
                    DataHoraExpiracao = servicoTokenJwt.ObterDataHoraExpiracao(),
                    EhProfessor = usuario.EhProfessor(),
                    EhProfessorCj = usuario.EhProfessorCj(),
                    EhProfessorPoa = usuario.EhProfessorPoa()
                };
            }
        }

        public async Task<UsuarioReinicioSenhaDto> ReiniciarSenha(string codigoRf)
        {
            var usuario = await servicoEOL.ObterMeusDados(codigoRf);

            var retorno = new UsuarioReinicioSenhaDto();

            if (String.IsNullOrEmpty(usuario.Email))
                retorno.DeveAtualizarEmail = true;
            else
            {
                await servicoEOL.ReiniciarSenha(codigoRf);
                retorno.DeveAtualizarEmail = false;
            }

            return retorno;
        }

        private void AdicionarHistoricoEmailUsuario(string login, string codigoRf, string email, AcaoHistoricoEmailUsuario acao)
        {
            var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(codigoRf, login);

            repositorioHistoricoEmailUsuario.Salvar(new HistoricoEmailUsuario()
            {
                UsuarioId = usuario.Id,
                Email = email,
                Acao = acao
            });
        }

        public async Task<RevalidacaoTokenDto> RevalidarLogin()
        {
            // Obter Login do token atual
            var login = servicoUsuario.ObterLoginAtual();
            string codigoRfAtual = servicoUsuario.ObterRf();
            string nomeLoginAtual = servicoUsuario.ObterNomeLoginAtual();

            var dadosUsuario = await servicoEOL.ObterMeusDados(login);
            var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(codigoRfAtual, login, nomeLoginAtual, dadosUsuario.Email);

            // Obter Perfil do token atual
            var guidPerfil = servicoTokenJwt.ObterPerfil();

            // Busca lista de permissões do EOL
            var permissionamentos = await servicoEOL.ObterPermissoesPorPerfil(guidPerfil);
            if (permissionamentos == null || !permissionamentos.Any())
                return null;

            var listaPermissoes = permissionamentos
                .Distinct()
                .Select(a => (Permissao)a)
                .ToList();

            await servicoTokenJwt.RevogarToken(login);

            return new RevalidacaoTokenDto()
            {
                Token = servicoTokenJwt.GerarToken(login, nomeLoginAtual, codigoRfAtual, guidPerfil, listaPermissoes),
                DataHoraExpiracao = servicoTokenJwt.ObterDataHoraExpiracao()
            };
        }

        public void Sair()
        {
            var login = servicoUsuario.ObterLoginAtual();
            var chaveRedis = $"perfis-usuario-{login}";
            repositorioCache.SalvarAsync(chaveRedis, string.Empty);
        }

        public async Task<string> SolicitarRecuperacaoSenha(string login)
        {
            var usuario = repositorioUsuario.ObterPorCodigoRfLogin(null, login);
            var usuarioCore = await servicoEOL.ObterMeusDados(login);

            if (usuarioCore == null)
                throw new NegocioException("Usuário não encontrado.");

            if (usuario == null)
                usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(usuarioCore.CodigoRf, null, usuarioCore.Nome, usuarioCore.Email);

            if (usuario.Perfis == null || !usuario.Perfis.Any())
                await servicoEOL.RelecionarUsuarioPerfis(login);

            usuario.DefinirPerfis(await servicoUsuario.ObterPerfisUsuario(login));
            usuario.DefinirEmail(usuarioCore.Email);
            usuario.IniciarRecuperacaoDeSenha();
            repositorioUsuario.Salvar(usuario);
            EnviarEmailRecuperacao(usuario, usuarioCore.Email);
            return usuarioCore.Email;
        }

        public bool TokenRecuperacaoSenhaEstaValido(Guid token)
        {
            Usuario usuario = repositorioUsuario.ObterPorTokenRecuperacaoSenha(token);
            return usuario != null && usuario.TokenRecuperacaoSenhaEstaValido();
        }

        private void EnviarEmailRecuperacao(Usuario usuario, string email)
        {
            string caminho = $"{Directory.GetCurrentDirectory()}/wwwroot/ModelosEmail/RecuperacaoSenha.txt";
            var textoArquivo = File.ReadAllText(caminho);
            var urlFrontEnd = configuration["UrlFrontEnd"];
            var textoEmail = textoArquivo
                .Replace("#NOME", usuario.Nome)
                .Replace("#RF", usuario.CodigoRf)
                .Replace("#URL_BASE#", urlFrontEnd)
                .Replace("#LINK", $"{urlFrontEnd}redefinir-senha/{usuario.TokenRecuperacaoSenha.ToString()}");
            servicoEmail.Enviar(email, "Recuperação de senha do SGP", textoEmail);
        }

        private async Task<IEnumerable<Guid>> ValidarPerfilCJ(string codigoRF, Guid codigoUsuarioCore, IEnumerable<Guid> perfilsAtual, string login)
        {
            var atribuicaoCJ = repositorioAtribuicaoCJ.ObterAtribuicaoAtiva(codigoRF);

            if (atribuicaoCJ != null && atribuicaoCJ.Any())
                return perfilsAtual;

            var atribuicaoEsporadica = repositorioAtribuicaoEsporadica.ObterUltimaPorRF(codigoRF);

            if (atribuicaoEsporadica != null && !string.IsNullOrWhiteSpace(atribuicaoEsporadica.ProfessorRf) && atribuicaoEsporadica.DataFim > DateTime.Today)
                return perfilsAtual;

            await servicoEOL.RemoverCJSeNecessario(codigoUsuarioCore);

            var usuarioEol = await servicoEOL.ObterPerfisPorLogin(login);

            return usuarioEol.Perfis;
        }
    }
}