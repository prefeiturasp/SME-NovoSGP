using MediatR;
using Moq;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PlanejamentoAnual.Copiar
{
    public class MigrarPlanejamentoAnualUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly MigrarPlanejamentoAnualUseCase _useCase;

        public MigrarPlanejamentoAnualUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new MigrarPlanejamentoAnualUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Dto_Valido_Deve_Enviar_Comando_E_Retornar_True_()
        {
            var dto = new MigrarPlanejamentoAnualDto
            {
                ComponenteCurricularId = 1,
                TurmasDestinoIds = new List<long> { 10 },
                PlanejamentoPeriodosEscolaresIds = new List<long> { 100 }
            };

            _mediatorMock.Setup(m => m.Send(It.Is<MigrarPlanejamentoAnualCommand>(c => c.Planejamento == dto), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            var resultado = await _useCase.Executar(dto);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.Is<MigrarPlanejamentoAnualCommand>(c => c.Planejamento == dto), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Dto_Nulo_Deve_Enviar_Comando_E_Retornar_False_()
        {
            MigrarPlanejamentoAnualDto dto = null;

            _mediatorMock.Setup(m => m.Send(It.Is<MigrarPlanejamentoAnualCommand>(c => c.Planejamento == null), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(false);

            var resultado = await _useCase.Executar(dto);

            Assert.False(resultado);
            _mediatorMock.Verify(m => m.Send(It.Is<MigrarPlanejamentoAnualCommand>(c => c.Planejamento == null), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
