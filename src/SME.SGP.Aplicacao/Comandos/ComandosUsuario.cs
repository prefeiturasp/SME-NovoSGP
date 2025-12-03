using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosUsuario : IComandosUsuario
    {
        private readonly IRepositorioCache repositorioCache;
        private readonly IRepositorioHistoricoEmailUsuario repositorioHistoricoEmailUsuario;
        private readonly IRepositorioSuporteUsuario repositorioSuporteUsuario;
        private readonly IServicoAbrangencia servicoAbrangencia;
        private readonly IServicoAutenticacao servicoAutenticacao;
        private readonly IServicoPerfil servicoPerfil;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IMediator mediator;

        public ComandosUsuario(IServicoAutenticacao servicoAutenticacao,
            IServicoUsuario servicoUsuario,
            IServicoPerfil servicoPerfil,
            IRepositorioCache repositorioCache,
            IServicoAbrangencia servicoAbrangencia,
            IRepositorioHistoricoEmailUsuario repositorioHistoricoEmailUsuario,
            IRepositorioSuporteUsuario repositorioSuporteUsuario,
            IMediator mediator)
        {
            this.servicoAutenticacao = servicoAutenticacao ?? throw new ArgumentNullException(nameof(servicoAutenticacao));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.servicoPerfil = servicoPerfil ?? throw new ArgumentNullException(nameof(servicoPerfil));
            this.servicoAbrangencia = servicoAbrangencia ?? throw new ArgumentNullException(nameof(servicoAbrangencia));
            this.repositorioHistoricoEmailUsuario = repositorioHistoricoEmailUsuario ?? throw new ArgumentNullException(nameof(repositorioHistoricoEmailUsuario));
            this.repositorioSuporteUsuario = repositorioSuporteUsuario ?? throw new ArgumentNullException(nameof(repositorioSuporteUsuario));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        // TODO: aplicar validações permissão de acesso
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
            var usuario = await servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(null, login);
            
            if (usuario.EhNulo())
                throw new NegocioException("Usuário não encontrado.");

            usuario.ValidarSenha(alterarSenhaDto.NovaSenha);
            await servicoAutenticacao.AlterarSenha(login, alterarSenhaDto.SenhaAtual, alterarSenhaDto.NovaSenha);
        }

        public async Task<UsuarioAutenticacaoRetornoDto> AlterarSenhaComTokenRecuperacao(RecuperacaoSenhaDto recuperacaoSenhaDto)
        {
            var login = await mediator.Send(new AlterarSenhaComTokenRecuperacaoCommand(recuperacaoSenhaDto.Token, recuperacaoSenhaDto.NovaSenha));
            return await Autenticar(login, recuperacaoSenhaDto.NovaSenha);
        }

        public async Task<AlterarSenhaRespostaDto> AlterarSenhaPrimeiroAcesso(PrimeiroAcessoDto primeiroAcessoDto)
        {
            var usuario = new Usuario
            {
                Login = primeiroAcessoDto.Usuario
            };

            usuario.ValidarSenha(primeiroAcessoDto.NovaSenha);

            return await mediator.Send(new AlterarSenhaUsuarioCommand(usuario.Login, primeiroAcessoDto.NovaSenha));
        }

        public async Task<UsuarioAutenticacaoRetornoDto> Autenticar(string login, string senha)
        {
            login = login.Trim().ToLower();
            var retornoAutenticacaoEol = await servicoAutenticacao.AutenticarNoEol(login, senha);

            return await ObterAutenticacao(retornoAutenticacaoEol, login);
        }

        public async Task<UsuarioAutenticacaoRetornoDto> ObterAutenticacao((UsuarioAutenticacaoRetornoDto UsuarioAutenticacaoRetornoDto, string CodigoRf, IEnumerable<Guid> Perfis, 
                                                                            bool PossuiCargoCJ, bool PossuiPerfilCJ) retornoAutenticacaoEol, string login, SuporteUsuario suporte = null)
        {
            if (!retornoAutenticacaoEol.UsuarioAutenticacaoRetornoDto.Autenticado)
                return retornoAutenticacaoEol.UsuarioAutenticacaoRetornoDto;

            var dadosUsuario = await mediator.Send(new ObterUsuarioCoreSSOQuery(login));

            var usuario = await servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(retornoAutenticacaoEol.CodigoRf, login, dadosUsuario.Nome, dadosUsuario.Email, true);

            retornoAutenticacaoEol.UsuarioAutenticacaoRetornoDto.PerfisUsuario = await servicoPerfil.DefinirPerfilPrioritario(retornoAutenticacaoEol.Perfis, usuario);

            var perfis = retornoAutenticacaoEol.UsuarioAutenticacaoRetornoDto.PerfisUsuario.Perfis.Select(x => x.CodigoPerfil).ToList();
            await servicoAbrangencia.RemoverAbrangenciasHistoricasIncorretas(login, perfis);

            var perfilSelecionado = retornoAutenticacaoEol.UsuarioAutenticacaoRetornoDto.PerfisUsuario.PerfilSelecionado;

            var administradorSuporte = ObterAdministradorSuporte(suporte, usuario);

            var dadosAcesso = await mediator.Send(new CarregarDadosAcessoPorLoginPerfilQuery(login, perfilSelecionado, administradorSuporte));

            var permissionamentos = dadosAcesso.Permissoes.ToList();

            if (!permissionamentos.Any())
            {
                retornoAutenticacaoEol.UsuarioAutenticacaoRetornoDto.Autenticado = false;
                return retornoAutenticacaoEol.UsuarioAutenticacaoRetornoDto;
            }

            retornoAutenticacaoEol.UsuarioAutenticacaoRetornoDto.Token = dadosAcesso.Token;
            retornoAutenticacaoEol.UsuarioAutenticacaoRetornoDto.DataHoraExpiracao = dadosAcesso.DataExpiracaoToken;

            usuario.AtualizaUltimoLogin();

            await SalvarCacheUsuario(usuario);
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.AtualizaUltimoLoginUsuario, usuario, usuarioLogado: usuario));

            //await mediator.Send(new CarregarAbrangenciaUsuarioCommand(login, perfilSelecionado));

            retornoAutenticacaoEol.UsuarioAutenticacaoRetornoDto.UsuarioLogin = usuario.Login;
            retornoAutenticacaoEol.UsuarioAutenticacaoRetornoDto.UsuarioRf = usuario.CodigoRf;
            retornoAutenticacaoEol.UsuarioAutenticacaoRetornoDto.AdministradorSuporte = administradorSuporte;

            return retornoAutenticacaoEol.UsuarioAutenticacaoRetornoDto;
        }

        private Task SalvarCacheUsuario(Usuario usuario)
            => mediator.Send(new SalvarCachePorValorObjetoCommand(string.Format(NomeChaveCache.USUARIO, usuario.Login)
                                                                , usuario));

        private AdministradorSuporteDto ObterAdministradorSuporte(SuporteUsuario suporte, Usuario usuarioSimulado)
        {
            if (suporte.NaoEhNulo() && suporte.UsuarioPodeReceberSuporte(usuarioSimulado))
            {
                return new AdministradorSuporteDto
                {
                   Login = suporte.Administrador.Login,
                   Nome = suporte.Administrador.Nome
                };
            }

            return null;
        }

        public async Task<TrocaPerfilDto> ModificarPerfil(Guid perfil)
        {
            var loginAtual = await mediator.Send(ObterLoginAtualQuery.Instance);

            await servicoUsuario.PodeModificarPerfil(perfil, loginAtual);

            var administradorSuporte = new AdministradorSuporteDto
            {
                Login = servicoUsuario.ObterClaim("login_adm_suporte"),
                Nome = servicoUsuario.ObterClaim("nome_adm_suporte")
            };

            var dadosAcesso = await mediator.Send(new CarregarDadosAcessoPorLoginPerfilQuery(loginAtual, perfil, administradorSuporte));
            var permissionamentos = dadosAcesso.Permissoes.ToList();

            if (permissionamentos.EhNulo() || !permissionamentos.Any())
                throw new NegocioException("Não foi possível obter os permissionamentos do perfil selecionado");

            await servicoAbrangencia.Salvar(loginAtual, perfil, false);
            
            var usuario = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            usuario.DefinirPerfilAtual(perfil);

            return new TrocaPerfilDto
            {
                Token = dadosAcesso.Token,
                DataHoraExpiracao = dadosAcesso.DataExpiracaoToken,
                EhProfessor = usuario.EhProfessor(),
                EhProfessorCj = usuario.EhProfessorCj(),
                EhProfessorPoa = usuario.EhProfessorPoa(),
                EhProfessorInfantil = usuario.EhProfessorInfantil(),
                EhProfessorCjInfantil = usuario.EhProfessorCjInfantil(),
                EhPerfilProfessor = usuario.EhPerfilProfessor()
            };
        }

        public async Task<UsuarioReinicioSenhaDto> ReiniciarSenha(string codigoRf)
        {
            var usuario = await mediator.Send(new ObterUsuarioCoreSSOQuery(codigoRf));

            var retorno = new UsuarioReinicioSenhaDto();

            if (usuario.NaoEhNulo() && string.IsNullOrEmpty(usuario.Email))
                retorno.DeveAtualizarEmail = true;
            else
            {
                await mediator.Send(new ReiniciarSenhaEolCommand(codigoRf));
                retorno.DeveAtualizarEmail = false;
            }

            return retorno;
        }

        private void AdicionarHistoricoEmailUsuario(string login, string codigoRf, string email, AcaoHistoricoEmailUsuario acao)
        {
            var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(codigoRf, login);

            repositorioHistoricoEmailUsuario.Salvar(new HistoricoEmailUsuario
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

            // Obter Perfil do token atual
            var guidPerfil = await mediator.Send(ObterPerfilDoTokenQuery.Instance);

            // Busca lista de permissões do EOL
            var dadosAcesso = await mediator.Send(new CarregarDadosAcessoPorLoginPerfilQuery(login, guidPerfil));
            var permissionamentos = dadosAcesso.Permissoes.ToList();
            
            if (!permissionamentos.Any())
                return null;

            return new RevalidacaoTokenDto
            {
                Token = dadosAcesso.Token,
                DataHoraExpiracao = dadosAcesso.DataExpiracaoToken
            };
        }

        public void Sair()
        {
            var login = servicoUsuario.ObterLoginAtual();
            servicoUsuario.RemoverPerfisUsuarioCache(login);
        }

        public Task<string> SolicitarRecuperacaoSenha(string login)
        {
            var loginRecuperar = login.Replace(" ", "");
            return mediator.Send(new RecuperarSenhaCommand(loginRecuperar));
        }

        public async Task<bool> TokenRecuperacaoSenhaEstaValido(Guid token)
        {
            return await mediator.Send(new ValidarTokenRecuperacaoSenhaCommand(token));
        }

        public async Task<UsuarioAutenticacaoRetornoDto> AutenticarSuporte(string login)
        {
            login = login.Trim().ToLower();

            var retornoAutenticacaoEol = await servicoAutenticacao.AutenticarNoEolSemSenha(login);

            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            var suporte = new SuporteUsuario
            {
                Administrador = usuarioLogado,
                UsuarioAdministrador = usuarioLogado.Login,
                UsuarioSimulado = login,
                DataAcesso = DateTime.Now
            };

            var dto = await ObterAutenticacao(retornoAutenticacaoEol, login, suporte);

            suporte.TokenAcesso = dto.Token;

            repositorioSuporteUsuario.Salvar(suporte);

            return dto;
        }
    }
}