using MediatR;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class ExcluirCartaIntencoesObservacaoUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ExcluirCartaIntencoesObservacaoUseCase useCase;

        public ExcluirCartaIntencoesObservacaoUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ExcluirCartaIntencoesObservacaoUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Excluir_Observacao_Carta_De_Intenções()
        {
            //Arrange
            mediator.Setup(a => a.Send(It.IsAny<ExcluirCartaIntencoesObservacaoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            //Act
            var retorno = await useCase.Executar(1);

            //Asert
            mediator.Verify(x => x.Send(It.IsAny<ExcluirCartaIntencoesObservacaoCommand>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(retorno);
        }
    }
}
