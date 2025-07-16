using MediatR;
using Moq;
using SME.SGP.Infra.Dtos;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Fechamento
{
    public class ObterFechamentoIdPorTurmaBimestreUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterFechamentoIdPorTurmaBimestreUseCase _useCase;

        public ObterFechamentoIdPorTurmaBimestreUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterFechamentoIdPorTurmaBimestreUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Fechamento_Turma_Periodo_Escolar_Dto_Corretamente()
        {
            var turmaId = 1234L;
            var bimestre = 2;
            var param = new TurmaBimestreDto(turmaId, bimestre);

            var fechamentoEsperado = new FechamentoTurmaPeriodoEscolarDto
            {
                FechamentoTurmaId = 5678,
                PeriodoEscolarId = 91011
            };

            _mediatorMock
                .Setup(m => m.Send(
                    It.Is<ObterFechamentoTurmaComPeriodoEscolarQuery>(
                        q => q.TurmaId == turmaId && q.Bimestre == bimestre),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(fechamentoEsperado);

            var resultado = await _useCase.Executar(param);

            Assert.NotNull(resultado);
            Assert.Equal(fechamentoEsperado.FechamentoTurmaId, resultado.FechamentoTurmaId);
            Assert.Equal(fechamentoEsperado.PeriodoEscolarId, resultado.PeriodoEscolarId);

            _mediatorMock.Verify(m => m.Send(
                It.Is<ObterFechamentoTurmaComPeriodoEscolarQuery>(
                    q => q.TurmaId == turmaId && q.Bimestre == bimestre),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
