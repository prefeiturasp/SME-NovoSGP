using MediatR;
using Moq;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class ObterPlanejamentoAnualPorTurmaComponentePeriodoEscolarUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterPlanejamentoAnualPorTurmaComponentePeriodoEscolarUseCase useCase;

        public ObterPlanejamentoAnualPorTurmaComponentePeriodoEscolarUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterPlanejamentoAnualPorTurmaComponentePeriodoEscolarUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Retornar_o_Planejamento_Sem_Componentes()
        {
            var turmaId = 1L;
            var componenteCurricularId = 1L;
            var periodoEscolarId = 1L;

            var mockRetorno = new PlanejamentoAnualPeriodoEscolarDto
            {
                Componentes = null,
                PeriodoEscolarId = 1,
                Bimestre = 1,
            };

            mediator.Setup(a => a.Send(It.IsAny<ObterPlanejamentoAnualPorTurmaComponentePeriodoEscolarQuery>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(mockRetorno);

            var retorno = await useCase.Executar(turmaId, componenteCurricularId, periodoEscolarId);

            mediator.Verify(x => x.Send(It.Is<ObterPlanejamentoAnualPorTurmaComponentePeriodoEscolarQuery>(q =>
                q.TurmaId == turmaId &&
                q.ComponenteCurricularId == componenteCurricularId &&
                q.PeriodoEscolarId == periodoEscolarId), It.IsAny<CancellationToken>()), Times.Once);

            Assert.NotNull(retorno);
            Assert.Null(retorno.Componentes);
        }

        [Fact]
        public async Task Deve_Retornar_o_Planejamento_Com_Componentes()
        {
            var turmaId = 1L;
            var componenteCurricularId = 2L;
            var periodoEscolarId = 1L;

            var mockRetorno = new PlanejamentoAnualPeriodoEscolarDto
            {
                Componentes = new List<PlanejamentoAnualComponenteDto>() {
                    new PlanejamentoAnualComponenteDto {
                    ComponenteCurricularId = 2,
                        Descricao = "TESTE",
                        ObjetivosAprendizagemId = new List<long> {1,2,3 },
                        Auditoria = new AuditoriaDto()
                    }
                },
                PeriodoEscolarId = 1,
                Bimestre = 1,
            };

            mediator.Setup(a => a.Send(It.IsAny<ObterPlanejamentoAnualPorTurmaComponentePeriodoEscolarQuery>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(mockRetorno);

            var retorno = await useCase.Executar(turmaId, componenteCurricularId, periodoEscolarId);

            mediator.Verify(x => x.Send(It.Is<ObterPlanejamentoAnualPorTurmaComponentePeriodoEscolarQuery>(q =>
                q.TurmaId == turmaId &&
                q.ComponenteCurricularId == componenteCurricularId &&
                q.PeriodoEscolarId == periodoEscolarId), It.IsAny<CancellationToken>()), Times.Once);

            Assert.NotNull(retorno);
            Assert.NotNull(retorno.Componentes);
        }

        [Fact]
        public void Deve_Lancar_ArgumentNullException_Quando_Mediator_For_Nulo()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new ObterPlanejamentoAnualPorTurmaComponentePeriodoEscolarUseCase(null));

            Assert.Equal("mediator", exception.ParamName);
        }
    }
}