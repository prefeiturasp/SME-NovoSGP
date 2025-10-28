using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNotas.VisaoUe;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoUe;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Testes.Aplicacao.PainelEducacional
{
    public class ConsultasModalidadesNotasVisaoUeUseCaseTestes
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ConsultasModalidadesNotasVisaoUeUseCase useCase;

        public ConsultasModalidadesNotasVisaoUeUseCaseTestes()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ConsultasModalidadesNotasVisaoUeUseCase(mediatorMock.Object);
        }

        [Fact(DisplayName = "Deve retornar lista de IdentificacaoInfo com nomes baseados no enum Modalidade")]
        public async Task Deve_Retornar_Lista_Com_Nomes_Corretos()
        {
            // Arrange
            var modalidadesIds = new List<IdentificacaoInfo>
            {
                new() { Id = (int)Modalidade.Fundamental },
                new() { Id = (int)Modalidade.Medio }
            };

            mediatorMock
                .Setup(x => x.Send(It.IsAny<ObterModalidadesNotasVisaoUeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(modalidadesIds);

            // Act
            var resultado = await useCase.ObterModalidadesNotasVisaoUe(2025, "UE123", 1);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().HaveCount(2);
            resultado.First().Nome.Should().Be("Ensino Fundamental");
            resultado.Last().Nome.Should().Be("Ensino Médio");

            mediatorMock.Verify(x =>
                x.Send(It.IsAny<ObterModalidadesNotasVisaoUeQuery>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact(DisplayName = "Deve retornar lista vazia quando não houver modalidades")]
        public async Task Deve_Retornar_Lista_Vazia_Quando_Sem_Resultados()
        {
            // Arrange
            mediatorMock
                .Setup(x => x.Send(It.IsAny<ObterModalidadesNotasVisaoUeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<IdentificacaoInfo>());

            // Act
            var resultado = await useCase.ObterModalidadesNotasVisaoUe(2025, "UE999", 2);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().BeEmpty();
        }

        [Fact(DisplayName = "Deve chamar o mediator com parâmetros corretos")]
        public async Task Deve_Chamar_Mediator_Com_Parametros_Corretos()
        {
            // Arrange
            var anoLetivo = 2025;
            var codigoUe = "UE001";
            var bimestre = 3;
            mediatorMock
                .Setup(x => x.Send(It.IsAny<ObterModalidadesNotasVisaoUeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<IdentificacaoInfo>());

            // Act
            await useCase.ObterModalidadesNotasVisaoUe(anoLetivo, codigoUe, bimestre);

            // Assert
            mediatorMock.Verify(x =>
                x.Send(It.Is<ObterModalidadesNotasVisaoUeQuery>(
                    q => q.AnoLetivo == anoLetivo &&
                         q.CodigoUe == codigoUe &&
                         q.Bimestre == bimestre),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
