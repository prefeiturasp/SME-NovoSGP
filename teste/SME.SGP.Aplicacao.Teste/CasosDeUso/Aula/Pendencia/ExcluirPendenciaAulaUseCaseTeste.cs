using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Aula.Pendencia
{
    public class ExcluirPendenciaAulaUseCaseTeste
    {
        [Fact]
        public async Task Executar_DeveEnviarCommandERetornarTrue()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();

            var dto = new ExcluirPendenciaAulaDto
            {
                AulaId = 321,
                TipoPendenciaAula = TipoPendencia.Avaliacao
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ExcluirPendenciaAulaCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var useCase = new ExcluirPendenciaAulaUseCase(mediatorMock.Object);

            // Act
            var resultado = await useCase.Executar(dto);

            // Assert
            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.Is<ExcluirPendenciaAulaCommand>(
                c => c.AulaId == dto.AulaId && c.TipoPendenciaAula == dto.TipoPendenciaAula
            ), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
