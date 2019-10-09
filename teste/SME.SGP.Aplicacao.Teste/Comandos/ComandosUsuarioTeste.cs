using Microsoft.Extensions.Configuration;
using Moq;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Servicos;
using SME.SGP.Dominio.Interfaces;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Comandos
{
    public class ComandosUsuarioTeste
    {
        private readonly ComandosUsuario comandosUsuario;
        private readonly Mock<IRepositorioUsuario> repositorioUsuario;
        private readonly Mock<IServicoAutenticacao> servicoAutenticacao;
        private readonly Mock<IServicoEmail> servicoEmail;
        private readonly Mock<IServicoEOL> servicoEOL;
        private readonly Mock<IServicoPerfil> servicoPerfil;
        private readonly Mock<IServicoTokenJwt> servicoTokenJwt;
        private readonly Mock<IServicoUsuario> servicoUsuario;

        public ComandosUsuarioTeste()
        {
            repositorioUsuario = new Mock<IRepositorioUsuario>();
            servicoAutenticacao = new Mock<IServicoAutenticacao>();
            servicoUsuario = new Mock<IServicoUsuario>();
            servicoPerfil = new Mock<IServicoPerfil>();
            servicoTokenJwt = new Mock<IServicoTokenJwt>();
            servicoEOL = new Mock<IServicoEOL>();
            servicoEmail = new Mock<IServicoEmail>();
            var mockConfiguration = new Mock<IConfiguration>();
            comandosUsuario = new ComandosUsuario(repositorioUsuario.Object, servicoAutenticacao.Object, servicoUsuario.Object, servicoPerfil.Object, servicoEOL.Object, servicoTokenJwt.Object, servicoEmail.Object, mockConfiguration.Object);
        }

        [Fact]
        public async void Deve_Atualizar_Email()
        {
            //ARRANGE
            var codifoRfTeste = "codigoRfTeste";
            servicoUsuario.Setup(a => a.ObterUsuarioPorCodigoRfLoginOuAdiciona(codifoRfTeste, string.Empty)).Returns(new Dominio.Usuario() { Email = "emaildeteste@teste.com" });
            servicoEOL.Setup(a => a.ReiniciarSenha(codifoRfTeste));

            //ACT
            var retorno = await comandosUsuario.ReiniciarSenha(codifoRfTeste);

            //ASSERT
            Assert.True(!retorno.DeveAtualizarEmail);
        }

        //public async void Deve_Atualizar_Email_Por_Login()
        //{
        //    //ARRANGE
        //    var loginTeste = "loginTeste";
        //    var usuario = new Usuario() { Email = "teste@teste.com" };
        //    repositorioUsuario.Setup(a => a.ObterPorCodigoRfLogin(string.Empty, loginTeste)).Returns(usuario);
        //    repositorioUsuario.Setup(a => a.ExisteUsuarioComMesmoEmail(loginTeste, 0)).Returns(false);

        //    servicoUsuario.Setup(a => a.AlterarEmailUsuarioPorLogin(loginTeste, "jose@jose.com"))
        //        .Returns(Task.FromResult(usuario));

        //    servicoEOL.Setup(a => a.ObterPerfisPorLogin(loginTeste)).Returns(Task.FromResult(new Dto.UsuarioEolAutenticacaoRetornoDto() { Status = Dto.AutenticacaoStatusEol.Ok }));

        //    //ACT
        //    var retorno = await comandosUsuario.ReiniciarSenha(codifoRfTeste);

        //    //ASSERT
        //    Assert.True(retorno.DeveAtualizarEmail);
        //}

        [Fact]
        public async void Deve_Retornar_Atualizar_Email_Ao_Reiniciar_Senha()
        {
            //ARRANGE
            var codifoRfTeste = "codigoRfTeste";
            servicoUsuario.Setup(a => a.ObterUsuarioPorCodigoRfLoginOuAdiciona(codifoRfTeste, string.Empty)).Returns(new Dominio.Usuario());
            //ACT
            var retorno = await comandosUsuario.ReiniciarSenha(codifoRfTeste);

            //ASSERT
            Assert.True(retorno.DeveAtualizarEmail);
        }
    }
}