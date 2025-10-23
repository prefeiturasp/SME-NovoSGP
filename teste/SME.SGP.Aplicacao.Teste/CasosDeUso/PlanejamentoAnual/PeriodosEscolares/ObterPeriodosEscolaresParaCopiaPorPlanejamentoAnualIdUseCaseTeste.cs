using MediatR;
using Moq;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class ObterPeriodosEscolaresParaCopiaPorPlanejamentoAnualIdUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterPeriodosEscolaresParaCopiaPorPlanejamentoAnualIdUseCase useCase;

        public ObterPeriodosEscolaresParaCopiaPorPlanejamentoAnualIdUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterPeriodosEscolaresParaCopiaPorPlanejamentoAnualIdUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Lista_Com_Quatro_Bimestres()
        {
            var planejamentoAnualId = 1L;
            var mockRetorno = new List<PlanejamentoAnualPeriodoEscolarResumoDto>
            {
                new PlanejamentoAnualPeriodoEscolarResumoDto
                {
                    Id = 1,
                    Bimestre = 1
                },new PlanejamentoAnualPeriodoEscolarResumoDto
                {
                    Id = 5,
                    Bimestre = 2
                },new PlanejamentoAnualPeriodoEscolarResumoDto
                {
                    Id = 10,
                    Bimestre = 3
                },new PlanejamentoAnualPeriodoEscolarResumoDto
                {
                    Id = 11,
                    Bimestre = 4
                }
            };

            mediator.Setup(a => a.Send(It.IsAny<ObterPeriodosEscolaresPorPlanejamentoAnualIdQuery>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(mockRetorno);


            var retorno = await useCase.Executar(planejamentoAnualId);

            mediator.Verify(x => x.Send(It.Is<ObterPeriodosEscolaresPorPlanejamentoAnualIdQuery>(q => q.PlanejamentoAnualId == planejamentoAnualId), It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(retorno.Any());
        }

        [Fact]
        public void Deve_Lancar_ArgumentNullException_Quando_Mediator_For_Nulo()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new ObterPeriodosEscolaresParaCopiaPorPlanejamentoAnualIdUseCase(null));

            Assert.Equal("mediator", exception.ParamName);
        }
    }
}