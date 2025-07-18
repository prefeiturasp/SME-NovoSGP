using Bogus;
using MediatR;
using Moq;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class ObterEncaminhamentoAEESituacoesUseCaseTeste
    {
        private readonly ObterEncaminhamentoAEESituacoesUseCase _useCase;
        private readonly Mock<IMediator> _mediator;
        private readonly Faker _faker;

        public ObterEncaminhamentoAEESituacoesUseCaseTeste()
        {
            _mediator = new Mock<IMediator>();
            _useCase = new ObterEncaminhamentoAEESituacoesUseCase(_mediator.Object);
            _faker = new Faker("pt_BR");
        }

        [Fact]
        public async Task Deve_Obter_EncaminhamentosAEE_Situacoes()
        {
            // Arrange
            var filtro = new FiltroDashboardAEEDto
            {
                AnoLetivo = 0,
                DreId = _faker.Random.Long(),
                UeId = _faker.Random.Long()
            };

            var situacoes = new List<AEESituacaoEncaminhamentoDto>();
            for (int i = 0; i < 2; i++)
            {
                situacoes.Add(new AEESituacaoEncaminhamentoDto
                {
                    Quantidade = _faker.Random.Int(),
                    Situacao = _faker.PickRandom<SituacaoAEE>()
                });
            }

            var retornoEsperado = new DashboardAEEEncaminhamentosDto
            {
                QtdeEncaminhamentosSituacao = 8,
                TotalEncaminhamentosAnalise = 5,
                SituacoesEncaminhamentoAEE = situacoes
            };

            _mediator.Setup(m => m.Send(It.IsAny<ObterEncaminhamentoAEESituacoesQuery>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(retornoEsperado);

            // Act
            var resultado = await _useCase.Executar(filtro);

            // Assert
            Assert.NotNull(resultado);
        }
    }
}
