using MediatR;
using Moq;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.AtividadeInfantil
{
    public class ObterAtividadesInfantilUseCaseTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterAtividadesInfantilUseCase _useCase;

        public ObterAtividadesInfantilUseCaseTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterAtividadesInfantilUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DeveRetornarAtividadesInfantisPorAulaId()
        {
            // Arrange
            long aulaId = 123;
            var atividadesEsperadas = new List<AtividadeInfantilDto>
            {
                new AtividadeInfantilDto { Id = 1, Titulo = "Atividade 1" },
                new AtividadeInfantilDto { Id = 2, Titulo = "Atividade 2" }
            };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAtividadesInfantilPorAulaIdQuery>(q => q.AulaId == aulaId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(atividadesEsperadas);

            // Act
            var result = await _useCase.Executar(aulaId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(atividadesEsperadas, result);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterAtividadesInfantilPorAulaIdQuery>(q => q.AulaId == aulaId), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveRetornarListaVaziaQuandoNaoHaAtividades()
        {
            // Arrange
            long aulaId = 456;
            var atividadesVazias = new List<AtividadeInfantilDto>();

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAtividadesInfantilPorAulaIdQuery>(q => q.AulaId == aulaId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(atividadesVazias);

            // Act
            var result = await _useCase.Executar(aulaId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterAtividadesInfantilPorAulaIdQuery>(q => q.AulaId == aulaId), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DevePropagarExcecaoQuandoOcorrerErroNaQuery()
        {
            // Arrange
            long aulaId = 789;
            var exceptionMessage = "Erro na comunicação com o repositório.";

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAtividadesInfantilPorAulaIdQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new System.Exception(exceptionMessage));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<System.Exception>(() => _useCase.Executar(aulaId));
            Assert.Contains(exceptionMessage, ex.Message);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterAtividadesInfantilPorAulaIdQuery>(q => q.AulaId == aulaId), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }
    }
}