using MediatR;
using Moq;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PlanejamentoAnual.Salvar
{
    public class SalvarPlanejamentoAnualUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly SalvarPlanejamentoAnualUseCase useCase;

        public SalvarPlanejamentoAnualUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new SalvarPlanejamentoAnualUseCase(mediatorMock.Object);
        }

        [Fact]
        public void Construtor_Quando_Mediator_Nulo_Deve_Lancar_Argument_Null_Exception()
        {
            Assert.Throws<ArgumentNullException>("mediator", () => new SalvarPlanejamentoAnualUseCase(null));
        }

        [Fact]
        public async Task Executar_Quando_Parametros_Validos_Deve_Enviar_Comando_E_Retornar_Auditoria()
        {
            long turmaId = 123;
            long componenteCurricularId = 456;
            var dto = new SalvarPlanejamentoAnualDto
            {
                PeriodosEscolares = new List<PlanejamentoAnualPeriodoEscolarDto>
                {
                    new PlanejamentoAnualPeriodoEscolarDto
                    {
                        PeriodoEscolarId = 789,
                        Bimestre = 1
                    }
                }
            };

            var auditoriaRetorno = new PlanejamentoAnualAuditoriaDto { Id = 1 };

            mediatorMock.Setup(m => m.Send(
                It.Is<SalvarPlanejamentoAnualCommand>(cmd =>
                    cmd.TurmaId == turmaId &&
                    cmd.ComponenteCurricularId == componenteCurricularId &&
                    cmd.PeriodosEscolares == dto.PeriodosEscolares),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(auditoriaRetorno);

            var resultado = await useCase.Executar(turmaId, componenteCurricularId, dto);

            Assert.NotNull(resultado);
            Assert.Equal(auditoriaRetorno.Id, resultado.Id);
            mediatorMock.Verify(m => m.Send(
                It.Is<SalvarPlanejamentoAnualCommand>(cmd =>
                    cmd.TurmaId == turmaId &&
                    cmd.ComponenteCurricularId == componenteCurricularId),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
