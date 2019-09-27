using Microsoft.Extensions.Configuration;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.IO;
using System.Reflection;

namespace SME.SGP.Aplicacao
{
    public class ComandoAuth : IComandoAuth
    {
        private readonly IConfiguration configuration;
        private readonly IRepositorioUsuario repositorioUsuario;

        public ComandoAuth(IRepositorioUsuario repositorioUsuario,
                           IConfiguration configuration)
        {
            this.repositorioUsuario = repositorioUsuario;
            this.configuration = configuration ?? throw new System.ArgumentNullException(nameof(configuration));
        }

        public string RecuperarSenha(string codigo)
        {
            var usuario = repositorioUsuario.ObterPorCodigoRfLogin(codigo, null);
            if (usuario == null)
            {
                throw new NegocioException("Usuário não encontrado.");
            }
            usuario.IniciarRecuperacaoDeSenha();
            repositorioUsuario.Salvar(usuario);
            EnviarEmailRecuperacao(usuario);

            return usuario.Email;
        }

        private void EnviarEmailRecuperacao(Usuario usuario)
        {
            string caminho = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"ModelosEmail\RecuperacaoSenha.txt");
            var textoArquivo = File.ReadAllText(caminho);
            var urlFrontEnd = configuration["UrlFrontEnt"];
            var textoEmail = textoArquivo
                .Replace("#NOME", usuario.Nome)
                .Replace("#RF", usuario.CodigoRf)
                .Replace("#LINK", $"{urlFrontEnd}redefinir/{usuario.TokenRecuperacaoSenha.ToString()}");
            //TODO - Enviar E-mail
        }
    }
}