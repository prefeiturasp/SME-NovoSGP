using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PlanoAEE
{
    public class ObterRestruturacoesPlanoAEEPorIdUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterRestruturacoesPlanoAEEPorIdUseCase useCase;

        public ObterRestruturacoesPlanoAEEPorIdUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterRestruturacoesPlanoAEEPorIdUseCase(mediator.Object);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Lista_De_Restruturacoes()
        {
            // Arrange
            var planoId = 1L;
            var reestruturacoesEsperadas = new List<PlanoAEEReestruturacaoDto>
            {
                new PlanoAEEReestruturacaoDto { Id = 1, Descricao = "Reestruturação 1" },
                new PlanoAEEReestruturacaoDto { Id = 2, Descricao = "Reestruturação 2" }
            };

            mediator.Setup(x => x.Send(
                It.Is<ObterRestruturacoesPlanoAEEQuery>(q => q.PlanoId == planoId),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(reestruturacoesEsperadas);

            // Act
            var resultado = await useCase.Executar(planoId);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.Equal("Reestruturação 1", resultado.First().Descricao);
        }

        [Fact]
        public async Task Executar_Com_PlanoId_Invalido_Deve_Retornar_Lista_Vazia()
        {
            // Arrange
            var planoIdInvalido = 0L;

            mediator.Setup(x => x.Send(
                It.Is<ObterRestruturacoesPlanoAEEQuery>(q => q.PlanoId == planoIdInvalido),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PlanoAEEReestruturacaoDto>());

            // Act
            var resultado = await useCase.Executar(planoIdInvalido);

            // Assert
            Assert.NotNull(resultado);
            Assert.Empty(resultado);
        }

        [Fact]
        public async Task Executar_Deve_Chamar_Mediator_Com_PlanoId_Correto()
        {
            // Arrange
            var planoId = 2L;

            mediator.Setup(x => x.Send(It.IsAny<ObterRestruturacoesPlanoAEEQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PlanoAEEReestruturacaoDto>());

            // Act
            await useCase.Executar(planoId);

            // Assert
            mediator.Verify(x => x.Send(
                It.Is<ObterRestruturacoesPlanoAEEQuery>(q => q.PlanoId == planoId),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Mediator_Lanca_Excecao_Deve_Repassar()
        {
            // Arrange
            var planoId = 1L;
            mediator.Setup(x => x.Send(It.IsAny<ObterRestruturacoesPlanoAEEQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new System.Exception("Erro simulado"));

            // Act & Assert
            await Assert.ThrowsAsync<System.Exception>(() => useCase.Executar(planoId));
        }
    }
}
