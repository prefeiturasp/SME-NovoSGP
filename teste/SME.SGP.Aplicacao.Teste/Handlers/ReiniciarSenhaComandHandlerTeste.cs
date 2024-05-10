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
        private readonly Mock<IMediator> mediator;
        private readonly ReiniciarSenhaCommandHandler reiniciarSenhaCommandHandler;

        public ReiniciarSenhaCommandHandlerTeste()
        {
            mediator = new Mock<IMediator>();
            reiniciarSenhaCommandHandler = new ReiniciarSenhaCommandHandler(mediator.Object);
        }

        [Theory]
        [InlineData("user.test", "", "", "Sgp@1234")]
        [InlineData("111111", "", "", "Sgp1111")]
        [InlineData("222222", "", "", "Sgp2222")]
        [InlineData("333333", "", "", "Sgp3333")]
        public async Task Deve_Reiniciar_A_Senha(string codigoRf, string dreCodigo, string ueCodigo, string resultadoSenha)
        {
            //Arrange
            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioCoreSSOQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new MeusDadosDto() { CodigoRf = codigoRf, Email = "teste@teste.com.br" });
            mediator.Setup(a => a.Send(It.IsAny<ReiniciarSenhaEolCommand>(), It.IsAny<CancellationToken>()));
            
            mediator.Setup(a => a.Send(It.IsAny<GravarHistoricoReinicioSenhaCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await reiniciarSenhaCommandHandler.Handle(new ReiniciarSenhaCommand(codigoRf, dreCodigo, ueCodigo), new CancellationToken());

            //Assert
            Assert.False(result.DeveAtualizarEmail);
            Assert.Contains(resultadoSenha, result.Mensagem);
        }
    }
}
