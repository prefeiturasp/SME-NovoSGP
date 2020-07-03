using Microsoft.Extensions.Configuration;
using Moq;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao.Teste.Comandos
{
    public class ComandosUsuarioTeste
    {
        private readonly ComandosUsuario comandosUsuario;
        private readonly Mock<IRepositorioAtribuicaoCJ> repositorioAtribuicaoCJ;
        private readonly Mock<IRepositorioAtribuicaoEsporadica> repositorioAtribuicaoEsporadica;
        private readonly Mock<IRepositorioHistoricoEmailUsuario> repositorioHistoricoEmailUsuario;
        private readonly Mock<IRepositorioUsuario> repositorioUsuario;
        private readonly Mock<IServicoAbrangencia> servicoAbrangencia;
        private readonly Mock<IServicoAutenticacao> servicoAutenticacao;
        private readonly Mock<IServicoEmail> servicoEmail;
        private readonly Mock<IServicoEol> servicoEOL;
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
            servicoEOL = new Mock<IServicoEol>();
            var repositorioCache = new Mock<IRepositorioCache>();
            servicoEmail = new Mock<IServicoEmail>();
            var mockConfiguration = new Mock<IConfiguration>();
            servicoAbrangencia = new Mock<IServicoAbrangencia>();
            repositorioAtribuicaoEsporadica = new Mock<IRepositorioAtribuicaoEsporadica>();
            repositorioAtribuicaoCJ = new Mock<IRepositorioAtribuicaoCJ>();
            repositorioHistoricoEmailUsuario = new Mock<IRepositorioHistoricoEmailUsuario>();

            comandosUsuario = new ComandosUsuario(repositorioUsuario.Object, servicoAutenticacao.Object, servicoUsuario.Object, servicoPerfil.Object, servicoEOL.Object, servicoTokenJwt.Object, servicoEmail.Object,
                mockConfiguration.Object, repositorioCache.Object, servicoAbrangencia.Object, repositorioAtribuicaoEsporadica.Object, repositorioAtribuicaoCJ.Object, repositorioHistoricoEmailUsuario.Object);
        }

        //[Fact]
        //public async void Deve_Atualizar_Email()
        //{
        //    //ARRANGE
        //    var codifoRfTeste = "codigoRfTeste";
        //    servicoUsuario.Setup(a => a.ObterUsuarioPorCodigoRfLoginOuAdiciona(codifoRfTeste, string.Empty, string.Empty, string.Empty, false)).Returns(new Dominio.Usuario());
        //    servicoEOL.Setup(a => a.ReiniciarSenha(codifoRfTeste));

        //    //ACT
        //    var retorno = await comandosUsuario.ReiniciarSenha(codifoRfTeste);

        //    //ASSERT
        //    Assert.True(!retorno.DeveAtualizarEmail);
        //}

        //[Fact]
        //public async void Deve_Atualizar_Email_Por_Login()
        //{
        //    //ARRANGE
        //    var codigoRfTeste = "loginTeste";

        //    servicoUsuario.Setup(a => a.ObterLoginAtual()).Returns(codigoRfTeste);
        //    servicoUsuario.Setup(a => a.AlterarEmailUsuarioPorRfOuInclui(codigoRfTeste, "jose@jose.com"));

        //    //ACT
        //    await comandosUsuario.AlterarEmailUsuarioLogado("jose@jose.com");

        //    //ASSERT
        //    Assert.True(true);
        //}

        //[Fact]
        //public async void Deve_Atualizar_Email_Por_RF()
        //{
        //    //ARRANGE
        //    var codigoRfTeste = "loginTeste";

        //    servicoUsuario.Setup(a => a.AlterarEmailUsuarioPorRfOuInclui(codigoRfTeste, "jose@jose.com"));

        //    //ACT
        //    await comandosUsuario.AlterarEmail(new AlterarEmailDto() { NovoEmail = "jose@jose.com" }, codigoRfTeste);

        //    //ASSERT
        //    Assert.True(true);
        //}

        //[Fact]
        //public async void Deve_Retornar_Atualizar_Email_Ao_Reiniciar_Senha()
        //{
        //    //ARRANGE
        //    var codifoRfTeste = "codigoRfTeste";
        //    servicoUsuario.Setup(a => a.ObterUsuarioPorCodigoRfLoginOuAdiciona(codifoRfTeste, string.Empty, string.Empty, string.Empty, false)).Returns(new Dominio.Usuario());
        //    //ACT
        //    var retorno = await comandosUsuario.ReiniciarSenha(codifoRfTeste);

        //    //ASSERT
        //    Assert.True(retorno.DeveAtualizarEmail);
        //}
    }
}