using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        private readonly IRepositorioSuporteUsuario repositorioSuporteUsuario;
        private readonly IServicoAbrangencia servicoAbrangencia;
        private readonly IServicoAutenticacao servicoAutenticacao;
        private readonly IServicoEol servicoEOL;
        private readonly IServicoPerfil servicoPerfil;
        private readonly IServicoTokenJwt servicoTokenJwt;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IMediator mediator;

        public ComandosUsuario(IRepositorioUsuario repositorioUsuario,
            IServicoAutenticacao servicoAutenticacao,
            IServicoUsuario servicoUsuario,
            IServicoPerfil servicoPerfil,
            IServicoEol servicoEOL,
            IServicoTokenJwt servicoTokenJwt,
            IConfiguration configuration,
            IRepositorioCache repositorioCache,
            IServicoAbrangencia servicoAbrangencia,
            IRepositorioAtribuicaoEsporadica repositorioAtribuicaoEsporadica,
            IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ,
            IRepositorioHistoricoEmailUsuario repositorioHistoricoEmailUsuario,
            IRepositorioSuporteUsuario repositorioSuporteUsuario,
            IMediator mediator)
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
            this.repositorioSuporteUsuario = repositorioSuporteUsuario ?? throw new ArgumentNullException(nameof(repositorioSuporteUsuario));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
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
            var usuario = await servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(null, login);
            
            if (usuario == null)
            {
                throw new NegocioException("Usuário não encontrado.");
            }
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
            var usuario = new Usuario();

            usuario.Login = primeiroAcessoDto.Usuario;

            usuario.ValidarSenha(primeiroAcessoDto.NovaSenha);

            return await servicoEOL.AlterarSenha(usuario.Login, primeiroAcessoDto.NovaSenha);
        }

        public async Task<UsuarioAutenticacaoRetornoDto> Autenticar(string login, string senha)
        {
            login = login.Trim().ToLower();

            var retornoAutenticacaoEol = await servicoAutenticacao.AutenticarNoEol(login, senha);

            return await ObtenhaAutenticacao(retornoAutenticacaoEol, login);
        }

        private async Task<UsuarioAutenticacaoRetornoDto> ObtenhaAutenticacao((UsuarioAutenticacaoRetornoDto, string, IEnumerable<Guid>, bool, bool) retornoAutenticacaoEol, string login, List<Claim> claims = null)
        {
            if (!retornoAutenticacaoEol.Item1.Autenticado)
                return retornoAutenticacaoEol.Item1;

            var dadosUsuario = await servicoEOL.ObterMeusDados(login);

            var usuario = await servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(retornoAutenticacaoEol.Item2, login, dadosUsuario.Nome, dadosUsuario.Email, true);

            retornoAutenticacaoEol.Item1.PerfisUsuario = await servicoPerfil.DefinirPerfilPrioritario(retornoAutenticacaoEol.Item3, usuario);

            List<Guid> perfis = retornoAutenticacaoEol.Item1.PerfisUsuario.Perfis.Select(x => x.CodigoPerfil).ToList();
            servicoAbrangencia.RemoverAbrangenciasHistoricasIncorretas(login, perfis);

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

            // Gera novo token e guarda em cache
            retornoAutenticacaoEol.Item1.Token =
                servicoTokenJwt.GerarToken(login, dadosUsuario.Nome, usuario.CodigoRf, retornoAutenticacaoEol.Item1.PerfisUsuario.PerfilSelecionado, listaPermissoes, claims);

            retornoAutenticacaoEol.Item1.DataHoraExpiracao = servicoTokenJwt.ObterDataHoraExpiracao();

            usuario.AtualizaUltimoLogin();

            repositorioUsuario.Salvar(usuario);

            await servicoAbrangencia.Salvar(login, perfilSelecionado, true);

            retornoAutenticacaoEol.Item1.UsuarioLogin = usuario.Login;
            retornoAutenticacaoEol.Item1.UsuarioRf = usuario.CodigoRf;

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

                //await servicoTokenJwt.RevogarToken(loginAtual);
                var tokenStr = servicoTokenJwt.GerarToken(loginAtual, nomeLoginAtual, codigoRfAtual, perfil, listaPermissoes);

                return new TrocaPerfilDto
                {
                    Token = tokenStr,
                    DataHoraExpiracao = servicoTokenJwt.ObterDataHoraExpiracao(),
                    EhProfessor = usuario.EhProfessor(),
                    EhProfessorCj = usuario.EhProfessorCj(),
                    EhProfessorPoa = usuario.EhProfessorPoa(),
                    EhProfessorInfantil = usuario.EhProfessorInfantil(),
                    EhProfessorCjInfantil = usuario.EhProfessorCjInfantil(),
                    EhPerfilProfessor = usuario.EhPerfilProfessor()
                };
            }
        }

        public async Task<UsuarioReinicioSenhaDto> ReiniciarSenha(string codigoRf)
        {
            var usuario = await servicoEOL.ObterMeusDados(codigoRf);

            var retorno = new UsuarioReinicioSenhaDto();

            if (usuario != null && String.IsNullOrEmpty(usuario.Email))
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

            //await servicoTokenJwt.RevogarToken(login);

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

        public Task<string> SolicitarRecuperacaoSenha(string login)
        {
            string loginRecuperar = login.Replace(" ", "");
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

            var claims = new List<Claim>();
            claims.Add(new Claim("login_adm_suporte", usuarioLogado.Login));
            claims.Add(new Claim("nome_adm_suporte", usuarioLogado.Nome));

            var dto = await ObtenhaAutenticacao(retornoAutenticacaoEol, login, claims);

            repositorioSuporteUsuario.Salvar(new SuporteUsuario()
            {
                UsuarioAdministrador = usuarioLogado.Login,
                UsuarioSimulado = login,
                DataAcesso = DateTime.Now,
                TokenAcesso = dto.Token
            });

            return dto;
        }
        public async Task<UsuarioAutenticacaoRetornoDto> DeslogarSuporte()
        {
            var administrador = await mediator.Send(new ObterAdministradorDoSuporteQuery());

            if (administrador == null || string.IsNullOrEmpty(administrador.Login))
            {
                throw new NegocioException($"O usuário não está em suporte de um administrador!");
            }

            var retornoAutenticacaoEol = await servicoAutenticacao.AutenticarNoEolSemSenha(administrador.Login);

            return await ObtenhaAutenticacao(retornoAutenticacaoEol, administrador.Login);
        }
    }
}