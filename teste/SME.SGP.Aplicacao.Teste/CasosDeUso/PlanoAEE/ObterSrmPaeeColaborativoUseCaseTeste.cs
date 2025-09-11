using MediatR;
using Moq;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PlanoAEE
{
    public class ObterSrmPaeeColaborativoUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterSrmPaeeColaborativoUseCase useCase;

        public ObterSrmPaeeColaborativoUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterSrmPaeeColaborativoUseCase(mediator.Object);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Dados_SrmPaee_Colaborativo()
        {
            // Arrange
            var filtro = new FiltroSrmPaeeColaborativoDto { CodigoAluno = 123 };
            var dadosEsperados = new List<SrmPaeeColaborativoSgpDto>
            {
                new SrmPaeeColaborativoSgpDto { DreUe = "1", ComponenteCurricular = "Dado 1" },
                new SrmPaeeColaborativoSgpDto { DreUe = "2", ComponenteCurricular = "Dado 2" }
            };

            mediator.Setup(x => x.Send(
                It.Is<ObterDadosSrmPaeeColaborativoEolQuery>(q => q.CodigoAluno == filtro.CodigoAluno),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(dadosEsperados);

            // Act
            var resultado = await useCase.Executar(filtro);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.Equal("Dado 1", resultado.First().ComponenteCurricular);
            Assert.Equal("Dado 2", resultado.Last().ComponenteCurricular);
        }

        [Fact]
        public async Task Executar_Com_Filtro_Nulo_Deve_Lancar_Excecao()
        {
            // Arrange
            mediator.Setup(x => x.Send(It.IsAny<ObterDadosSrmPaeeColaborativoEolQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<SrmPaeeColaborativoSgpDto>());

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => useCase.Executar(null));
        }

        [Fact]
        public async Task Executar_Com_CodigoAluno_Vazio_Deve_Retornar_Lista_Vazia()
        {
            // Arrange
            var filtro = new FiltroSrmPaeeColaborativoDto { CodigoAluno = 0 };

            mediator.Setup(x => x.Send(
                It.Is<ObterDadosSrmPaeeColaborativoEolQuery>(q => q.CodigoAluno == filtro.CodigoAluno),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<SrmPaeeColaborativoSgpDto>());

            // Act
            var resultado = await useCase.Executar(filtro);

            // Assert
            Assert.NotNull(resultado);
            Assert.Empty(resultado);
        }

        [Fact]
        public async Task Executar_Deve_Chamar_Mediator_Com_CodigoAluno_Correto()
        {
            // Arrange
            var filtro = new FiltroSrmPaeeColaborativoDto { CodigoAluno = 456 };

            mediator.Setup(x => x.Send(It.IsAny<ObterDadosSrmPaeeColaborativoEolQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<SrmPaeeColaborativoSgpDto>());

            // Act
            await useCase.Executar(filtro);

            // Assert
            mediator.Verify(x => x.Send(
                It.Is<ObterDadosSrmPaeeColaborativoEolQuery>(q => q.CodigoAluno == filtro.CodigoAluno),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Mediator_Lanca_Excecao_Deve_Repassar()
        {
            // Arrange
            var filtro = new FiltroSrmPaeeColaborativoDto { CodigoAluno = 789 };

            mediator.Setup(x => x.Send(It.IsAny<ObterDadosSrmPaeeColaborativoEolQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new System.Exception("Erro simulado"));

            // Act & Assert
            await Assert.ThrowsAsync<System.Exception>(() => useCase.Executar(filtro));
        }
    }
}
