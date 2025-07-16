using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Aula
{
    public class ObterPlanoAulasPorTurmaEComponentePeriodoUseCaseTeste
    {
        [Fact]
        public async Task Executar_DeveRetornarPlanoAulas_QuandoFiltroForValido()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();

            var filtro = new FiltroObterPlanoAulaPeriodoDto(
                turmaCodigo: "TURMA123",
                componenteCurricularCodigo: "456",
                componenteCurricularId: "789",
                aulaInicio: new DateTime(2025, 7, 1),
                aulaFim: new DateTime(2025, 7, 15)
            );

            var planosEsperados = new List<PlanoAulaRetornoDto>
            {
                new PlanoAulaRetornoDto
                {
                    AulaId = 1,
                    Descricao = "Plano Aula 1",
                    DataAula = new DateTime(2025, 7, 2)
                },
                new PlanoAulaRetornoDto
                {
                    AulaId = 2,
                    Descricao = "Plano Aula 2",
                    DataAula = new DateTime(2025, 7, 5)
                }
            };

            mediatorMock.Setup(m => m.Send(It.Is<ObterPlanoAulasPorTurmaEComponentePeriodoQuery>(query =>
                query.TurmaCodigo == filtro.TurmaCodigo &&
                query.ComponenteCurricularCodigo == filtro.ComponenteCurricularCodigo &&
                query.ComponenteCurricularId == filtro.ComponenteCurricularId &&
                query.AulaInicio == filtro.AulaInicio &&
                query.AulaFim == filtro.AulaFim
            ), It.IsAny<CancellationToken>())).ReturnsAsync(planosEsperados);

            var useCase = new ObterPlanoAulasPorTurmaEComponentePeriodoUseCase(mediatorMock.Object);

            // Act
            var resultado = await useCase.Executar(filtro);

            // Assert
            Assert.NotNull(resultado);
            var listaResultado = Assert.IsAssignableFrom<IEnumerable<PlanoAulaRetornoDto>>(resultado);
            Assert.Collection(listaResultado,
                plano =>
                {
                    Assert.Equal(1, plano.AulaId);
                    Assert.Equal("Plano Aula 1", plano.Descricao);
                    Assert.Equal(new DateTime(2025, 7, 2), plano.DataAula);
                },
                plano =>
                {
                    Assert.Equal(2, plano.AulaId);
                    Assert.Equal("Plano Aula 2", plano.Descricao);
                    Assert.Equal(new DateTime(2025, 7, 5), plano.DataAula);
                });
        }
    }
}
