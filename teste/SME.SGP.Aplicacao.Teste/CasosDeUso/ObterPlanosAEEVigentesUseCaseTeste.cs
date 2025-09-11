using Bogus;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class ObterPlanosAEEVigentesUseCaseTeste
    {
        private readonly ObterPlanosAEEVigentesUseCase _useCase;
        private readonly Mock<IMediator> _mediator;
        private readonly Faker _faker;

        public ObterPlanosAEEVigentesUseCaseTeste()
        {
            _mediator = new Mock<IMediator>();
            _useCase = new ObterPlanosAEEVigentesUseCase(_mediator.Object);
            _faker = new Faker("pt_BR");
        }

        [Fact]
        public async Task Obter_PlanosAEE_Vigentes()
        {
            // Arrange
            var filtro = new FiltroDashboardAEEDto
            {
                AnoLetivo = 0,
                DreId = _faker.Random.Long(1, 100),
                UeId = _faker.Random.Long(1, 100)
            };

            var planosVigentes = new List<AEETurmaDto>();
            for (int i = 0; i < 2; i++)
            {
                planosVigentes.Add(new AEETurmaDto
                {
                    Quantidade = _faker.Random.Int(1, 30),
                    Modalidade = _faker.PickRandom<Modalidade>(),
                    AnoTurma = $"{_faker.Random.Int(1,9)}º",
                    Nome = _faker.Name.FullName(),
                    Descricao = _faker.Lorem.Sentence()
                });
            }

            var retornoEsperado = new DashboardAEEPlanosVigentesDto
            {
                TotalPlanosVigentes = planosVigentes.Sum(p => p.Quantidade),
                PlanosVigentes = planosVigentes
            };

            _mediator.Setup(m => m.Send(It.IsAny<ObterPlanosAEEVigentesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(retornoEsperado);

            // Act
            var resultado = await _useCase.Executar(filtro);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(retornoEsperado.TotalPlanosVigentes, resultado.TotalPlanosVigentes);
        }
    }
}
