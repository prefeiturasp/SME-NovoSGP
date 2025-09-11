using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DashboardFrequencia
{
    public class ExecutaConsolidacaoDiariaDashBoardFrequenciaPorUeUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IRepositorioTurmaConsulta> _repositorioTurmaMock;
        private readonly ExecutaConsolidacaoDiariaDashBoardFrequenciaPorUeUseCase _useCase;

        public ExecutaConsolidacaoDiariaDashBoardFrequenciaPorUeUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _repositorioTurmaMock = new Mock<IRepositorioTurmaConsulta>();

            _useCase = new ExecutaConsolidacaoDiariaDashBoardFrequenciaPorUeUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DevePublicarFilaSgpParaCadaTurmaId()
        {
            // Arrange
            var filtro = new ConsolidacaoPorUeDashBoardFrequencia
            {
                AnoLetivo = 2025,
                UeCodigo = "U001",
                Mes = 3
            };

            var mensagemRabbit = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(filtro) // Passe o objeto diretamente
            };

            var turmasIds = new List<long> { 1, 2, 3 };

            // Mock do mediator para query de turmas
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterTurmasIdPorUeCodigoEAnoLetivoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmasIds);

            // Mock do mediator para comando de publicação na fila
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _useCase.Executar(mensagemRabbit);

            // Assert
            Assert.True(result);

            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(cmd =>
                cmd.Rota == RotasRabbitSgpFrequencia.RotaConsolidacaoDiariaDashBoardFrequenciaPorTurma &&
                cmd.Filtros is ConsolidacaoPorTurmaDashBoardFrequencia &&
                ((ConsolidacaoPorTurmaDashBoardFrequencia)cmd.Filtros).AnoLetivo == filtro.AnoLetivo &&
                ((ConsolidacaoPorTurmaDashBoardFrequencia)cmd.Filtros).Mes == filtro.Mes &&
                turmasIds.Contains(((ConsolidacaoPorTurmaDashBoardFrequencia)cmd.Filtros).TurmaId)
            ), It.IsAny<CancellationToken>()), Times.Exactly(turmasIds.Count));
            
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTurmasIdPorUeCodigoEAnoLetivoQuery>(), It.IsAny<CancellationToken>()), Times.Once);


        }

    }
}
