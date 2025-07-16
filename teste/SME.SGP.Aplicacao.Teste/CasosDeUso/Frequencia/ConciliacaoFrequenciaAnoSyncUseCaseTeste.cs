using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Frequencia
{
    public class ConciliacaoFrequenciaAnoSyncUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ConciliacaoFrequenciaAnoSyncUseCase useCase;

        public ConciliacaoFrequenciaAnoSyncUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ConciliacaoFrequenciaAnoSyncUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DeveEnviarComando_QuandoMensagemForValida()
        {
            // Arrange
            var dto = new ConciliacaoFrequenciaAnoSyncDto(2025);
            var mensagem = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(dto)
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ConciliacaoFrequenciaAnoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var resultado = await useCase.Executar(mensagem);

            // Assert
            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(
                It.Is<ConciliacaoFrequenciaAnoCommand>(c => c.AnoLetivo == 2025),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
