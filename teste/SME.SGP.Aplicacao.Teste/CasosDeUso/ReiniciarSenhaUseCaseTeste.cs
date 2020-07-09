using MediatR;
using Moq;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class ReiniciarSenhaUseCaseTeste
    {
        private readonly ReiniciarSenhaUseCase reiniciarSenhaUseCase;
        private readonly Mock<IMediator> mediator;

        public ReiniciarSenhaUseCaseTeste()
        {

            mediator = new Mock<IMediator>();
            reiniciarSenhaUseCase = new ReiniciarSenhaUseCase(mediator.Object);
        }

        [Theory]
        [InlineData("caique.amcom", "", "", "Sgpmcom")]
        [InlineData("7944560", "", "", "Sgp4560")]
        [InlineData("7924488", "", "", "Sgp4488")]
        [InlineData("6940145", "", "", "Sgp0145")]
        public async Task Deve_Reiniciar_A_Senha(string codigoRf, string dreCodigo, string ueCodigo, string resultadoSenha)
        {
            //Arrange
            mediator.Setup(a => a.Send(It.IsAny<ReiniciarSenhaCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new UsuarioReinicioSenhaDto() {  Mensagem = resultadoSenha, DeveAtualizarEmail = true} );

            //Act
            var result = await reiniciarSenhaUseCase.ReiniciarSenha(codigoRf, dreCodigo, ueCodigo);

            //Asert
            mediator.Verify(x => x.Send(It.IsAny<ReiniciarSenhaCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(result.DeveAtualizarEmail);
            Assert.Contains(resultadoSenha, result.Mensagem);
        }
    }
}


