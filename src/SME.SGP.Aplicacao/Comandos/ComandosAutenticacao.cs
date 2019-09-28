using Microsoft.Extensions.Configuration;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosAutenticacao : IComandosAutenticacao
    {
        private readonly IConfiguration configuration;
        private readonly IRepositorioUsuario repositorioUsuario;
        private readonly IServicoEmail servicoEmail;
        private readonly IServicoEOL servicoEOL;

        public ComandosAutenticacao(IRepositorioUsuario repositorioUsuario,
                                    IConfiguration configuration,
                                    IServicoEmail servicoEmail,
                                    IServicoEOL servicoEOL)
        {
            this.repositorioUsuario = repositorioUsuario ?? throw new System.ArgumentNullException(nameof(repositorioUsuario));
            this.configuration = configuration ?? throw new System.ArgumentNullException(nameof(configuration));
            this.servicoEmail = servicoEmail ?? throw new System.ArgumentNullException(nameof(servicoEmail));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
        }

        public async Task AlterarSenhaComTokenRecuperacao(Guid token, string novaSenha)
        {
            Usuario usuario = repositorioUsuario.ObterPorTokenRecuperacaoSenha(token);
            if (usuario == null)
            {
                throw new NegocioException("Usuário não encontrado.");
            }
            if (!usuario.TokenRecuperacaoSenhaEstaValido())
            {
                throw new NegocioException("Este link expirou. Clique em continuar para solicitar um novo link de recuperação de senha.");
            }

            usuario.ValidarSenha(novaSenha);

            await servicoEOL.AlterarSenha(usuario.Login, novaSenha);
        }

        public string SolicitarRecuperacaoSenha(string login)
        {
            var usuario = repositorioUsuario.ObterPorCodigoRfLogin(login, null);
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
            string caminho = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"ModelosEmail\RecuperacaoSenha.txt");
            var textoArquivo = File.ReadAllText(caminho);
            var urlFrontEnd = configuration["UrlFrontEnt"];
            var textoEmail = textoArquivo
                .Replace("#NOME", usuario.Nome)
                .Replace("#RF", usuario.CodigoRf)
                .Replace("#URL_BASE#", urlFrontEnd)
                .Replace("#LINK", $"{urlFrontEnd}redefinir/{usuario.TokenRecuperacaoSenha.ToString()}");

            servicoEmail.Enviar(usuario.Email, "Recuperação de senha do SGP", textoEmail);
        }
    }
}