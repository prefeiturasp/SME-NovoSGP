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
        private readonly Mock<IServicoEol> servicoEOL;
        private readonly Mock<IMediator> mediator;
        private readonly ReiniciarSenhaCommandHandler reiniciarSenhaCommandHandler;

        public ReiniciarSenhaCommandHandlerTeste()
        {
            servicoEOL = new Mock<IServicoEol>();
            mediator = new Mock<IMediator>();
            reiniciarSenhaCommandHandler = new ReiniciarSenhaCommandHandler(mediator.Object, servicoEOL.Object);
        }

        [Theory]
        [InlineData("caique.amcom", "", "", "Sgpmcom")]
        [InlineData("7944560", "", "", "Sgp4560")]
        [InlineData("7924488", "", "", "Sgp4488")]
        [InlineData("6940145", "", "", "Sgp0145")]
        public async Task Deve_Reiniciar_A_Senha(string codigoRf, string dreCodigo, string ueCodigo, string resultadoSenha)
        {
            //Arrange
            servicoEOL.Setup(a => a.ObterMeusDados(codigoRf)).ReturnsAsync(new MeusDadosDto() { CodigoRf = codigoRf, Email = "teste@teste.com.br" });
            servicoEOL.Setup(a => a.ReiniciarSenha(codigoRf));

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
