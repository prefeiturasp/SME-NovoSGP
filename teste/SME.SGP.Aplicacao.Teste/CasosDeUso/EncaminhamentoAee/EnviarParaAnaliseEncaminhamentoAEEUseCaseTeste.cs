using MediatR;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste
{
    public class EnviarParaAnaliseEncaminhamentoAEEUseCaseTeste
    {
        private readonly EnviarParaAnaliseEncaminhamentoAEEUseCase enviarParaAnaliseEncaminhamentoAEEUseCase;
        private readonly Mock<IMediator> mediator;

        public EnviarParaAnaliseEncaminhamentoAEEUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            enviarParaAnaliseEncaminhamentoAEEUseCase = new EnviarParaAnaliseEncaminhamentoAEEUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Enviar_Para_Analise_Encaminhamento()
        {
            //Arrange
            mediator.Setup(a => a.Send(It.IsAny<EnviarParaAnaliseEncaminhamentoAEECommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            //Act
            var encerramentoId = 6;
            var encerrado = await enviarParaAnaliseEncaminhamentoAEEUseCase.Executar(encerramentoId);

            //Asert
            mediator.Verify(x => x.Send(It.IsAny<EnviarParaAnaliseEncaminhamentoAEECommand>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(encerrado);
        }
    }
}
