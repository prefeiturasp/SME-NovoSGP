using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PlanoAEE
{
    public class ObterPAAIPorDreUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterPAAIPorDreUseCase useCase;

        public ObterPAAIPorDreUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterPAAIPorDreUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Lista_De_PAAIs_Por_Dre()
        {
            // Arrange
            var codigoDre = "123";
            var expectedResult = new List<SupervisorEscolasDreDto>
            {
                new SupervisorEscolasDreDto
                {
                    SupervisorId = "1",
                    SupervisorNome = "PAAI Teste",
                }
            };

            mediator.Setup(x => x.Send(
                It.Is<ObterPAAIPorDreQuery>(q =>
                    q.CodigoDre == codigoDre &&
                    q.TipoResponsavel == TipoResponsavelAtribuicao.PAAI),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await useCase.Executar(codigoDre);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            mediator.Verify(x => x.Send(
                It.Is<ObterPAAIPorDreQuery>(q =>
                    q.CodigoDre == codigoDre &&
                    q.TipoResponsavel == TipoResponsavelAtribuicao.PAAI),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Lista_Vazia_Quando_Nao_Encontrar_PAAIs()
        {
            // Arrange
            var codigoDre = "999";
            var expectedResult = new List<SupervisorEscolasDreDto>();

            mediator.Setup(x => x.Send(
                It.Is<ObterPAAIPorDreQuery>(q =>
                    q.CodigoDre == codigoDre),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await useCase.Executar(codigoDre);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Deve_Tratar_CodigoDre_Vazio_Ou_Nulo(string codigoDre)
        {
            // Arrange
            var expectedResult = new List<SupervisorEscolasDreDto>();

            mediator.Setup(x => x.Send(
                It.IsAny<ObterPAAIPorDreQuery>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await useCase.Executar(codigoDre);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
