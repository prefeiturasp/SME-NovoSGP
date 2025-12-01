using Bogus;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class ObterEncaminhamentosAEEDeferidosUseCaseTeste
    {
        private readonly ObterEncaminhamentosAEEDeferidosUseCase _useCase;
        private readonly Mock<IMediator> _mediator;
        private readonly Faker _faker;

        public ObterEncaminhamentosAEEDeferidosUseCaseTeste()
        {
            _mediator = new Mock<IMediator>();
            _useCase = new ObterEncaminhamentosAEEDeferidosUseCase(_mediator.Object);
            _faker = new Faker("pt_BR");
        }

        [Fact]
        public async Task Deve_Obter_Encaminhamentos_AEEDeferidos()
        {
            // Arrange
            var filtro = new FiltroDashboardAEEDto
            {
                AnoLetivo = 0,
                DreId = _faker.Random.Long(1),
                UeId = _faker.Random.Long(1)
            };

            var retornoQuery = new List<AEETurmaDto>();
            for (int i = 0; i < 2; i++)
            {
                retornoQuery.Add(new AEETurmaDto
                {
                    Nome = _faker.Name.FullName(),
                    AnoTurma = "2º ano",
                    Modalidade = _faker.PickRandom<Modalidade>(),
                    Quantidade = 5,
                    Descricao = _faker.Lorem.Sentences()
                });
            }

            // Act
            var resultado = await _useCase.Executar(filtro);

            // Assert
            Assert.NotNull(resultado);
            var lista = resultado.ToList();

            _mediator.Verify(m => m.Send(It.IsAny<ObterEncaminhamentosAEEDeferidosQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
