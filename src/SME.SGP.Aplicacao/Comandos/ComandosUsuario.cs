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
        private readonly IServicoEol servicoEOL;
        private readonly IServicoPerfil servicoPerfil;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IMediator mediator;

        public ComandosUsuario(IServicoAutenticacao servicoAutenticacao,
            IServicoUsuario servicoUsuario,
            IServicoPerfil servicoPerfil,
            IServicoEol servicoEOL,
            IRepositorioCache repositorioCache,
            IServicoAbrangencia servicoAbrangencia,
            IRepositorioHistoricoEmailUsuario repositorioHistoricoEmailUsuario,
            IRepositorioSuporteUsuario repositorioSuporteUsuario,
            IMediator mediator)
        {
            this.servicoAutenticacao = servicoAutenticacao ?? throw new ArgumentNullException(nameof(servicoAutenticacao));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.servicoPerfil = servicoPerfil ?? throw new ArgumentNullException(nameof(servicoPerfil));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
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
            
            if (usuario == null)
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

            return await servicoEOL.AlterarSenha(usuario.Login, primeiroAcessoDto.NovaSenha);
        }

        public async Task<UsuarioAutenticacaoRetornoDto> Autenticar(string login, string senha)
        {
            login = login.Trim().ToLower();
            var retornoAutenticacaoEol = await servicoAutenticacao.AutenticarNoEol(login, senha);

            return await ObterAutenticacao(retornoAutenticacaoEol, login);
        }

        public async Task<UsuarioAutenticacaoRetornoDto> ObterAutenticacao((UsuarioAutenticacaoRetornoDto, string, IEnumerable<Guid>, bool, bool)
            retornoAutenticacaoEol, string login, SuporteUsuario suporte = null)
        {
            if (!retornoAutenticacaoEol.Item1.Autenticado)
                return retornoAutenticacaoEol.Item1;

            var dadosUsuario = await servicoEOL.ObterMeusDados(login);

            var usuario = await servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(retornoAutenticacaoEol.Item2, login, dadosUsuario.Nome, dadosUsuario.Email, true);

            retornoAutenticacaoEol.Item1.PerfisUsuario = await servicoPerfil.DefinirPerfilPrioritario(retornoAutenticacaoEol.Item3, usuario);

            var perfis = retornoAutenticacaoEol.Item1.PerfisUsuario.Perfis.Select(x => x.CodigoPerfil).ToList();
            servicoAbrangencia.RemoverAbrangenciasHistoricasIncorretas(login, perfis);

            var perfilSelecionado = retornoAutenticacaoEol.Item1.PerfisUsuario.PerfilSelecionado;

            var administradorSuporte = ObterAdministradorSuporte(suporte, usuario);

            var dadosAcesso = await servicoEOL.CarregarDadosAcessoPorLoginPerfil(login, perfilSelecionado, administradorSuporte);

            var permissionamentos = dadosAcesso.Permissoes.ToList();

            if (!permissionamentos.Any())
            {
                retornoAutenticacaoEol.Item1.Autenticado = false;
                return retornoAutenticacaoEol.Item1;
            }

            retornoAutenticacaoEol.Item1.Token = dadosAcesso.Token;
            retornoAutenticacaoEol.Item1.DataHoraExpiracao = dadosAcesso.DataExpiracaoToken;

            usuario.AtualizaUltimoLogin();

            await SalvarCacheUsuario(usuario);
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.AtualizaUltimoLoginUsuario, usuario, usuarioLogado: usuario));

            await mediator.Send(new CarregarAbrangenciaUsuarioCommand(login, perfilSelecionado));

            retornoAutenticacaoEol.Item1.UsuarioLogin = usuario.Login;
            retornoAutenticacaoEol.Item1.UsuarioRf = usuario.CodigoRf;
            retornoAutenticacaoEol.Item1.AdministradorSuporte = administradorSuporte;

            return retornoAutenticacaoEol.Item1;
        }

        private Task SalvarCacheUsuario(Usuario usuario)
            => mediator.Send(new SalvarCachePorValorObjetoCommand(string.Format(NomeChaveCache.CHAVE_USUARIO, usuario.Login)
                                                                , usuario));

        private AdministradorSuporteDto ObterAdministradorSuporte(SuporteUsuario suporte, Usuario usuarioSimulado)
        {
            if (suporte != null && suporte.UsuarioPodeReceberSuporte(usuarioSimulado))
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
            var loginAtual = servicoUsuario.ObterLoginAtual();

            await servicoUsuario.PodeModificarPerfil(perfil, loginAtual);

            var administradorSuporte = new AdministradorSuporteDto
            {
                Login = servicoUsuario.ObterClaim("login_adm_suporte"),
                Nome = servicoUsuario.ObterClaim("nome_adm_suporte")
            };

            var dadosAcesso = await servicoEOL.CarregarDadosAcessoPorLoginPerfil(loginAtual, perfil, administradorSuporte);
            //aqui pode dar nullpointer em dadosAcesso nao?
            var permissionamentos = dadosAcesso.Permissoes.ToList();

            if (permissionamentos == null || !permissionamentos.Any())
                throw new NegocioException("Não foi possível obter os permissionamentos do perfil selecionado");

            await servicoAbrangencia.Salvar(loginAtual, perfil, false);
            
            var usuario = await servicoUsuario.ObterUsuarioLogado();

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
            var usuario = await servicoEOL.ObterMeusDados(codigoRf);

            var retorno = new UsuarioReinicioSenhaDto();

            if (usuario != null && string.IsNullOrEmpty(usuario.Email))
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
            var guidPerfil = await mediator.Send(new ObterPerfilDoTokenQuery());

            // Busca lista de permissões do EOL
            var dadosAcesso = await servicoEOL.CarregarDadosAcessoPorLoginPerfil(login, guidPerfil);
            //aqui pode dar nullpointer em dadosAcesso nao?


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
            var chaveCache = string.Format(NomeChaveCache.CHAVE_PERFIS_USUARIO, login);
            repositorioCache.SalvarAsync(chaveCache, string.Empty);
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

            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

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