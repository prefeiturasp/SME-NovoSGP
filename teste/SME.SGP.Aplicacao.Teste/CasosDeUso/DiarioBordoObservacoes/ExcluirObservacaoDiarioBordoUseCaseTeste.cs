using MediatR;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class ExcluirObservacaoDiarioBordoUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ExcluirObservacaoDiarioBordoUseCase excluirObservacaoDiarioBordoUseCase;

        public ExcluirObservacaoDiarioBordoUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            excluirObservacaoDiarioBordoUseCase = new ExcluirObservacaoDiarioBordoUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Excluir_Observacao_Diario_De_Bordo()
        {
            //Arrange
            mediator.Setup(a => a.Send(It.IsAny<ExcluirObservacaoDiarioBordoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            //Act
            var retorno = await excluirObservacaoDiarioBordoUseCase.Executar(1);

            //Asert
            mediator.Verify(x => x.Send(It.IsAny<ExcluirObservacaoDiarioBordoCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(retorno);
        }
    }
}
