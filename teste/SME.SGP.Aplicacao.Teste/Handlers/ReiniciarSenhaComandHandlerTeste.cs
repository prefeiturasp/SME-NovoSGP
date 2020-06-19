using MediatR;
using Moq;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Comandos
{
    public class ReiniciarSenhaCommandHandlerTeste
    {
        private readonly Mock<IComandosUsuario> comandosUsuario;
        private readonly Mock<IServicoEol> servicoEOL;
        private readonly ReiniciarSenhaCommandHandler reiniciarSenhaCommandHandler;

        public ReiniciarSenhaCommandHandlerTeste()
        {
            comandosUsuario = new Mock<IComandosUsuario>();
            servicoEOL = new Mock<IServicoEol>();
            reiniciarSenhaCommandHandler = new ReiniciarSenhaCommandHandler(comandosUsuario.Object, servicoEOL.Object);
        }

        [Theory]
        [InlineData("caique.amcom", "Sgpmcom")]
        [InlineData("7944560", "Sgp4560")]
        [InlineData("7924488", "Sgp4488")]
        [InlineData("6940145", "Sgp0145")]
        public async Task Deve_Reiniciar_A_Senha(string codigoRf, string resultadoSenha)
        {
            //Arrange
            servicoEOL.Setup(a => a.ObterMeusDados(codigoRf)).ReturnsAsync(new MeusDadosDto() { CodigoRf = codigoRf, Email = "teste@teste.com.br" });
            servicoEOL.Setup(a => a.ReiniciarSenha(codigoRf));
            comandosUsuario.Setup(u => u.ReiniciarSenha(codigoRf)).ReturnsAsync(new UsuarioReinicioSenhaDto() { });

            // Act
            var result = await reiniciarSenhaCommandHandler.Handle(new ReiniciarSenhaCommand(codigoRf), new CancellationToken());

            //Assert
            Assert.False(result.DeveAtualizarEmail);
            Assert.Contains(resultadoSenha, result.Mensagem);
        }
    }
}
