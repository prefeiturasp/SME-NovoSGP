using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste
{
    public class EncerrarPlanoAEEUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly EncerrarPlanoAEEUseCase _useCase;
        private readonly long _planoId;

        public EncerrarPlanoAEEUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new EncerrarPlanoAEEUseCase(_mediatorMock.Object);
            _planoId = 1;
        }

        [Fact]
        public void Construtor_MediatorNulo_DeveLancarArgumentNullException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => new EncerrarPlanoAEEUseCase(null));
            Assert.Equal("mediator", exception.ParamName);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(999)]
        public async Task Executar_DiferentesCenarios_DeveRetornarDtoCorreto(long planoId)
        {
            // Arrange
            var retornoEsperado = new RetornoEncerramentoPlanoAEEDto(planoId, SituacaoPlanoAEE.Encerrado);

            ConfigurarMediatorMock(retornoEsperado);

            // Act
            var resultado = await _useCase.Executar(planoId);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(retornoEsperado.PlanoId, resultado.PlanoId);

            VerificarChamadaCommand(planoId);
        }

        [Fact]
        public async Task Executar_MediatorRetornaNulo_DeveRetornarNulo()
        {
            // Arrange
            ConfigurarMediatorMock(null);

            // Act
            var resultado = await _useCase.Executar(_planoId);

            // Assert
            Assert.Null(resultado);
            VerificarChamadaCommand(_planoId);
        }

        [Fact]
        public async Task Executar_MediatorLancaException_DevePropagarException()
        {
            // Arrange
            var exceptionEsperada = new Exception("Erro ao encerrar plano");

            _mediatorMock.Setup(m => m.Send(It.IsAny<EncerrarPlanoAeeCommand>(), It.IsAny<CancellationToken>()))
                        .ThrowsAsync(exceptionEsperada);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _useCase.Executar(_planoId));
            Assert.Equal(exceptionEsperada.Message, exception.Message);
        }

        [Fact]
        public async Task Executar_DeveVerificarChamadaUnica()
        {
            // Arrange
            ConfigurarMediatorMock(new RetornoEncerramentoPlanoAEEDto(_planoId, SituacaoPlanoAEE.Encerrado));

            // Act
            await _useCase.Executar(_planoId);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.IsAny<EncerrarPlanoAeeCommand>(), It.IsAny<CancellationToken>()),
                Times.Once());
            _mediatorMock.VerifyNoOtherCalls();
        }

        private void ConfigurarMediatorMock(RetornoEncerramentoPlanoAEEDto retorno)
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<EncerrarPlanoAeeCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(retorno);
        }

        private void VerificarChamadaCommand(long planoId)
        {
            _mediatorMock.Verify(m => m.Send(It.Is<EncerrarPlanoAeeCommand>(c => c.PlanoId == planoId),
                It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}