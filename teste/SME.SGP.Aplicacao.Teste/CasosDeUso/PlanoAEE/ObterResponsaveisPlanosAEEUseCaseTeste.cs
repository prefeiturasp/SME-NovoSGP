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
    public class ObterResponsaveisPlanosAEEUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterResponsaveisPlanosAEEUseCase useCase;

        public ObterResponsaveisPlanosAEEUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterResponsaveisPlanosAEEUseCase(mediator.Object);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Lista_De_Responsaveis()
        {
            // Arrange
            var filtro = new FiltroPlanosAEEDto
            {
                DreId = 1,
                UeId = 1,
                TurmaId = 1
            };

            var responsaveisEsperados = new List<UsuarioEolRetornoDto>
            {
                new UsuarioEolRetornoDto { CodigoRf = "123", NomeServidor = "Responsável 1" },
                new UsuarioEolRetornoDto { CodigoRf = "456", NomeServidor = "Responsável 2" }
            };

            mediator.Setup(x => x.Send(
                It.Is<ObterResponsaveisPlanoAEEQuery>(q => q.DreId == filtro.DreId &&
                                                          q.UeId == filtro.UeId &&
                                                          q.TurmaId == filtro.TurmaId),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(responsaveisEsperados);

            // Act
            var resultado = await useCase.Executar(filtro);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.Equal("123", resultado.First().CodigoRf);
            Assert.Equal("Responsável 1", resultado.First().NomeServidor);
        }

        [Fact]
        public async Task Lista_Vazia_Retornar_Null()
        {
            // Arrange
            mediator.Setup(x => x.Send(
                It.Is<ObterResponsaveisPlanoAEEQuery>(q => q == null),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<UsuarioEolRetornoDto>());

            // Assert & Act
            await Assert.ThrowsAsync<NullReferenceException>(() => useCase.Executar(null));
        }

        [Fact]
        public async Task Executar_Deve_Chamar_Mediator_Com_Parametros_Corretos()
        {
            // Arrange
            var filtro = new FiltroPlanosAEEDto
            {
                DreId = 2,
                UeId = 3,
                TurmaId = 4
            };

            mediator.Setup(x => x.Send(It.IsAny<ObterResponsaveisPlanoAEEQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<UsuarioEolRetornoDto>());

            // Act
            await useCase.Executar(filtro);

            // Assert
            mediator.Verify(x => x.Send(
                It.Is<ObterResponsaveisPlanoAEEQuery>(q =>
                    q.DreId == filtro.DreId &&
                    q.UeId == filtro.UeId &&
                    q.TurmaId == filtro.TurmaId),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}