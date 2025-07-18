using Bogus;
using MediatR;
using Moq;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class ObterAlunosMatriculadosSRMPAEEUseCaseTeste
    {
        private readonly ObterAlunosMatriculadosSRMPAEEUseCase _useCase;
        private readonly Mock<IMediator> _mediator;
        private readonly Faker _faker;

        public ObterAlunosMatriculadosSRMPAEEUseCaseTeste()
        {
            _mediator = new Mock<IMediator>();
            _useCase = new ObterAlunosMatriculadosSRMPAEEUseCase(_mediator.Object);
            _faker = new Faker("pt_BR");
        }

        [Fact]
        public async Task Deve_Obter_AlunosMatriculadosSRMPAEE()
        {
            // Arrange
            var filtro = new FiltroDashboardAEEDto
            {
                AnoLetivo = DateTime.Now.Year,
                DreCodigo = "123",
                UeCodigo = "456"
            };

            var alunosMatriculados = new List<AlunosMatriculadosEolDto>();
            {
               for (int i = 0; i < 5; i++)
                {
                    alunosMatriculados.Add(new AlunosMatriculadosEolDto
                    {
                        Modalidade = _faker.Random.AlphaNumeric(6),
                        Turma = _faker.Random.String2(2),
                        Ano = "2025"
                    });
                };
            };

            _mediator.Setup(m => m.Send(It.IsAny<ObterAlunosMatriculadosPorAnoLetivoECCEolQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(alunosMatriculados);

            // Act
            var resultado = await _useCase.Executar(filtro);

            // Assert
            Assert.NotNull(resultado);
        }

    }
}
