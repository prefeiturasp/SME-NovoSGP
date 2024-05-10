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
        [InlineData("user.test", "", "", "Sgpteste")]
        [InlineData("111111", "", "", "Sgp1111")]
        [InlineData("222222", "", "", "Sgp2222")]
        [InlineData("333333", "", "", "Sgp3333")]
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


