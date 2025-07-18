using Bogus;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class ObterPlanoAEESituacoesUseCaseTeste
    {
        private readonly ObterPlanoAEESituacoesUseCase _useCase;
        private readonly Mock<IMediator> _mediator;
        private readonly Faker _faker;

        public ObterPlanoAEESituacoesUseCaseTeste()
        {
            _mediator = new Mock<IMediator>();
            _useCase = new ObterPlanoAEESituacoesUseCase(_mediator.Object);
            _faker = new Faker("pt_BR");
        }

        [Fact]
        public async Task Obter_PlanoAEE_Situacoes()
        {
            // Arrange
            var filtro = new FiltroDashboardAEEDto
            {
                AnoLetivo = 0,
                DreId = _faker.Random.Long(1, 100),
                UeId = _faker.Random.Long(1, 100)
            };

            var esperado = new DashboardAEEPlanosSituacaoDto
            {
                TotalPlanosVigentes = _faker.Random.Long(1, 20),
                SituacoesPlanos = new[] {
                    new AEESituacaoPlanoDto
                    {
                        Quantidade = _faker.Random.Long(1, 10),
                        Situacao = _faker.PickRandom<SituacaoPlanoAEE>(),
                    }
                }
            };

            _mediator.Setup(m => m.Send(
                    It.Is<ObterPlanoAEESituacoesQuery>(q =>
                        q.Ano == DateTime.Now.Year &&
                        q.DreId == filtro.DreId &&
                        q.UeId == filtro.UeId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(esperado);

            // Act
            var resultado = await _useCase.Executar(filtro);

            // Assert
            Assert.NotNull(resultado);
            _mediator.Verify(m => m.Send(It.IsAny<ObterPlanoAEESituacoesQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
