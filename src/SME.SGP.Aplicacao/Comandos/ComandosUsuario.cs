using Microsoft.Extensions.Configuration;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosUsuario : IComandosUsuario
    {
        private readonly IConfiguration configuration;
        private readonly IRepositorioCache repositorioCache;
        private readonly IRepositorioUsuario repositorioUsuario;
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
            IRepositorioCache repositorioCache)
        {
            this.repositorioUsuario = repositorioUsuario ??
                throw new System.ArgumentNullException(nameof(repositorioUsuario));
            this.servicoAutenticacao = servicoAutenticacao ??
                throw new System.ArgumentNullException(nameof(servicoAutenticacao));
            this.servicoUsuario = servicoUsuario ??
                throw new System.ArgumentNullException(nameof(servicoUsuario));
            this.servicoPerfil = servicoPerfil ??
                throw new System.ArgumentNullException(nameof(servicoPerfil));
            this.servicoEOL = servicoEOL ??
                throw new System.ArgumentNullException(nameof(servicoEOL));
            this.servicoTokenJwt = servicoTokenJwt ??
                throw new System.ArgumentNullException(nameof(servicoTokenJwt));
            this.servicoEmail = servicoEmail ?? throw new ArgumentNullException(nameof(servicoEmail));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        //  TODO: aplicar validações permissão de acesso
        public async Task AlterarEmail(AlterarEmailDto alterarEmailDto, string codigoRf)
        {
            await servicoUsuario.AlterarEmailUsuarioPorRfOuInclui(codigoRf, alterarEmailDto.NovoEmail);
        }

        public async Task AlterarEmailUsuarioLogado(string novoEmail)
        {
            var login = servicoUsuario.ObterLoginAtual();
            await servicoUsuario.AlterarEmailUsuarioPorLogin(login, novoEmail);
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
            var retornoAutenticacaoEol = await servicoAutenticacao.AutenticarNoEol(login, senha);

            if (retornoAutenticacaoEol.Item1.Autenticado)
            {
                var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(retornoAutenticacaoEol.Item2, login);

                retornoAutenticacaoEol.Item1.PerfisUsuario = servicoPerfil.DefinirPerfilPrioritario(retornoAutenticacaoEol.Item3, usuario);

                var permissionamentos = await servicoEOL.ObterPermissoesPorPerfil(retornoAutenticacaoEol.Item1.PerfisUsuario.PerfilSelecionado);

                if (permissionamentos == null || !permissionamentos.Any())
                {
                    retornoAutenticacaoEol.Item1.Autenticado = false;
                }
                else
                {
                    var listaPermissoes = permissionamentos
                        .Distinct()
                        .Select(a => (Permissao)a)
                        .ToList();

                    retornoAutenticacaoEol.Item1.Token = servicoTokenJwt.GerarToken(login, usuario.CodigoRf, listaPermissoes, retornoAutenticacaoEol.Item1.PerfisUsuario.PerfilSelecionado.ToString());

                    usuario.AtualizaUltimoLogin();
                    repositorioUsuario.Salvar(usuario);
                }
            }
            return retornoAutenticacaoEol.Item1;
        }

        public async Task<string> ModificarPerfil(string guid)
        {
            string loginAtual = servicoUsuario.ObterLoginAtual();
            string codigoRfAtual = servicoUsuario.ObterRf();

            await servicoUsuario.PodeModificarPerfil(guid, loginAtual);

            var permissionamentos = await servicoEOL.ObterPermissoesPorPerfil(Guid.Parse(guid));

            if (permissionamentos == null || !permissionamentos.Any())
            {
                throw new NegocioException($"Não foi possível obter os permissionamentos do perfil {guid}");
            }
            else
            {
                var listaPermissoes = permissionamentos
                    .Distinct()
                    .Select(a => (Permissao)a)
                    .ToList();

                return servicoTokenJwt.GerarToken(loginAtual, codigoRfAtual, listaPermissoes, guid);
            }
        }

        public async Task<UsuarioReinicioSenhaDto> ReiniciarSenha(string codigoRf)
        {
            var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(codigoRf);

            var retorno = new UsuarioReinicioSenhaDto();

            if (!usuario.PodeReiniciarSenha())
                retorno.DeveAtualizarEmail = true;
            else
            {
                await servicoEOL.ReiniciarSenha(codigoRf);
                retorno.DeveAtualizarEmail = false;
            }

            return retorno;
        }

        public void Sair()
        {
            var login = servicoUsuario.ObterLoginAtual();
            var chaveRedis = $"perfis-usuario-{login}";
            repositorioCache.SalvarAsync(chaveRedis, string.Empty);
        }

        public string SolicitarRecuperacaoSenha(string login)
        {
            var usuario = repositorioUsuario.ObterPorCodigoRfLogin(null, login);
            if (usuario == null)
            {
                throw new NegocioException("Usuário não encontrado.");
            }
            usuario.IniciarRecuperacaoDeSenha();
            repositorioUsuario.Salvar(usuario);
            EnviarEmailRecuperacao(usuario);
            return usuario.Email;
        }

        public bool TokenRecuperacaoSenhaEstaValido(Guid token)
        {
            Usuario usuario = repositorioUsuario.ObterPorTokenRecuperacaoSenha(token);
            return usuario != null && usuario.TokenRecuperacaoSenhaEstaValido();
        }

        private void EnviarEmailRecuperacao(Usuario usuario)
        {
            string caminho = $"{Directory.GetCurrentDirectory()}/wwwroot/ModelosEmail/RecuperacaoSenha.txt";
            var textoArquivo = File.ReadAllText(caminho);
            var urlFrontEnd = configuration["UrlFrontEnt"];
            var textoEmail = textoArquivo
                .Replace("#NOME", usuario.Nome)
                .Replace("#RF", usuario.CodigoRf)
                .Replace("#URL_BASE#", urlFrontEnd)
                .Replace("#LINK", $"{urlFrontEnd}redefinir-senha/{usuario.TokenRecuperacaoSenha.ToString()}");
            servicoEmail.Enviar(usuario.Email, "Recuperação de senha do SGP", textoEmail);
        }
    }
}