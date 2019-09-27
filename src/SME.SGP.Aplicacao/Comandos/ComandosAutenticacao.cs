using Microsoft.Extensions.Configuration;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.IO;
using System.Reflection;

namespace SME.SGP.Aplicacao
{
    public class ComandosAutenticacao : IComandosAutenticacao
    {
        private readonly IConfiguration configuration;
        private readonly IRepositorioUsuario repositorioUsuario;
        private readonly IServicoEmail servicoEmail;

        public ComandosAutenticacao(IRepositorioUsuario repositorioUsuario,
                                    IConfiguration configuration,
                                    IServicoEmail servicoEmail)
        {
            this.repositorioUsuario = repositorioUsuario ?? throw new System.ArgumentNullException(nameof(repositorioUsuario));
            this.configuration = configuration ?? throw new System.ArgumentNullException(nameof(configuration));
            this.servicoEmail = servicoEmail ?? throw new System.ArgumentNullException(nameof(servicoEmail));
        }

        public string RecuperarSenha(string usuario)
        {
            var usuarioLocalizado = repositorioUsuario.ObterPorCodigoRfLogin(usuario, null);
            if (usuarioLocalizado == null)
            {
                throw new NegocioException("Usuário não encontrado.");
            }
            usuarioLocalizado.IniciarRecuperacaoDeSenha();
            repositorioUsuario.Salvar(usuarioLocalizado);
            EnviarEmailRecuperacao(usuarioLocalizado);

            return usuarioLocalizado.Email;
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