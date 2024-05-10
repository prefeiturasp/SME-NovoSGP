using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste
{
    public class EncerrarEncaminhamentoAEEUseCaseTeste
    {
        private readonly EncerrarEncaminhamentoAEEUseCase encerrarEncaminhamentoAEEUseCase;
        private readonly Mock<IMediator> mediator;

        public EncerrarEncaminhamentoAEEUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            encerrarEncaminhamentoAEEUseCase = new EncerrarEncaminhamentoAEEUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Encerrar_Encaminhamento()
        {
            //Arrange
            mediator.Setup(a => a.Send(It.IsAny<EncerrarEncaminhamentoAEECommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            //Act
            var encerramentoId = 1;
            var motivoEncerramento = "Teste encerramento";
            var encerrado = await encerrarEncaminhamentoAEEUseCase.Executar(encerramentoId, motivoEncerramento);

            //Asert
            mediator.Verify(x => x.Send(It.IsAny<EncerrarEncaminhamentoAEECommand>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(encerrado);
        }
    }
}
