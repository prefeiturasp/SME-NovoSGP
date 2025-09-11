using Moq;
using Xunit;
using System;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DashboardFrequencia
{
    public class ExecutaConsolidacaoDiariaDashBoardFrequenciaDTOUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ExecutaConsolidacaoDiariaDashBoardFrequenciaDTOUseCase _useCase;

        public ExecutaConsolidacaoDiariaDashBoardFrequenciaDTOUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExecutaConsolidacaoDiariaDashBoardFrequenciaDTOUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DevePublicarFilaSgpCommand_ComSucesso()
        {
            // Arrange
            var filtroConsolicacao = new FiltroConsolicacaoDiariaDashBoardFrequenciaDTO
            {
                AnoLetivo = 2025,
                Mes = 7
            };

            // Act
            var resultado = await _useCase.Executar(filtroConsolicacao);

            // Assert
            Assert.True(resultado); // Verifica se o retorno é true

            // Verifica se o comando foi enviado para o mediator
            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(cmd =>
                cmd.Rota == RotasRabbitSgpFrequencia.RotaConsolidacaoDiariaDashBoardFrequencia &&
                cmd.Filtros == filtroConsolicacao &&
                cmd.CodigoCorrelacao != Guid.Empty), default), Times.Once);
        }
    }
}
