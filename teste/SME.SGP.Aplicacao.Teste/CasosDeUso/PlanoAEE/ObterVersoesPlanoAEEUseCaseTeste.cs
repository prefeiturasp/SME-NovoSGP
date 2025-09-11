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
    public class ObterVersoesPlanoAEEUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterVersoesPlanoAEEUseCase useCase;

        public ObterVersoesPlanoAEEUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterVersoesPlanoAEEUseCase(mediator.Object);
        }

        private FiltroVersoesPlanoAEEDto CriarFiltroVersoesPlanoAEE(long planId = 1, long reestruturacaoId = 1)
           => new FiltroVersoesPlanoAEEDto(planId, reestruturacaoId);


        [Fact]
        public async Task Executar_Deve_Retornar_Versoes_Formatadas_Corretamente()
        {
            // Arrange
            var filtro = CriarFiltroVersoesPlanoAEE();

            var versoesPlano = new List<PlanoAEEVersaoDto>
            {
                new PlanoAEEVersaoDto
                {
                    Id = 1,
                    Numero = 1,
                },
                new PlanoAEEVersaoDto
                {
                    Id = 2,
                    Numero = 2,
                }
            };

            mediator.Setup(x => x.Send(
                It.Is<ObterVersoesPlanoAEESemReestruturacaoQuery>(
                    q => q.PlanoId == filtro.PlanoId &&
                         q.ReestruturacaoId == filtro.ReestruturacaoId),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(versoesPlano);

            // Act
            var resultado = await useCase.Executar(filtro);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());

            var primeiraVersao = resultado.First();
            Assert.Equal(1, primeiraVersao.Id);

            var segundaVersao = resultado.Last();
            Assert.Equal(2, segundaVersao.Id);
        }

        [Fact]
        public async Task Executar_Com_Filtro_Nulo_Deve_Lancar_Excecao()
        {
            // Arrange
            mediator.Setup(x => x.Send(It.IsAny<ObterVersoesPlanoAEESemReestruturacaoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PlanoAEEVersaoDto>());

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => useCase.Executar(null));
        }

        [Fact]
        public async Task Executar_Com_Lista_Vazia_Deve_Retornar_Lista_Vazia()
        {
            // Arrange
            var filtro = CriarFiltroVersoesPlanoAEE();

            mediator.Setup(x => x.Send(
                It.IsAny<ObterVersoesPlanoAEESemReestruturacaoQuery>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PlanoAEEVersaoDto>());

            // Act
            var resultado = await useCase.Executar(filtro);

            // Assert
            Assert.NotNull(resultado);
            Assert.Empty(resultado);
        }

        [Fact]
        public async Task Executar_Deve_Chamar_Mediator_Com_Parametros_Corretos()
        {
            // Arrange
            var filtro = CriarFiltroVersoesPlanoAEE();

            mediator.Setup(x => x.Send(
                It.IsAny<ObterVersoesPlanoAEESemReestruturacaoQuery>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PlanoAEEVersaoDto>());

            // Act
            await useCase.Executar(filtro);

            // Assert
            mediator.Verify(x => x.Send(
                It.Is<ObterVersoesPlanoAEESemReestruturacaoQuery>(
                    q => q.PlanoId == filtro.PlanoId &&
                         q.ReestruturacaoId == filtro.ReestruturacaoId),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
