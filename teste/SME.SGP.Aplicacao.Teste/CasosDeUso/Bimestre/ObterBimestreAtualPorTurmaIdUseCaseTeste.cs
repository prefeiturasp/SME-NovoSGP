using MediatR;
using Moq;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Bimestre
{
    public class ObterBimestreAtualPorTurmaIdUseCaseTeste
    {
        [Fact]
        public void Constructor_Null_Mediator_Throws_Argument_Null_Exception()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new ObterBimestreAtualPorTurmaIdUseCase(null));
            Assert.Equal("mediator", ex.ParamName);
        }

        [Fact]
        public async Task Executar_Turma_Nao_Encontrada_Throws_Negocio_Exception()
        {
            var mockMediator = new Mock<IMediator>();

            mockMediator
                .Setup(m => m.Send(It.IsAny<ObterTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Turma)null);

            var useCase = new ObterBimestreAtualPorTurmaIdUseCase(mockMediator.Object);

            var ex = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(123));
            Assert.Equal("A turma informada não foi encontrada", ex.Message);
        }

        [Fact]
        public async Task Executar_Ano_Letivo_Diferente_Do_Not_Call_Periodo_Returns_Null()
        {
            var mockMediator = new Mock<IMediator>();

            var turma = new Turma
            {
                Id = 10,
                AnoLetivo = DateTime.Today.Year - 1 
            };

            mockMediator
                .Setup(m => m.Send(It.IsAny<ObterTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            mockMediator
                .Setup(m => m.Send(It.IsAny<ObterPeriodoEscolarAtualQuery>(), It.IsAny<CancellationToken>()))
                .Throws(new InvalidOperationException("Não deveria consultar período quando ano é diferente"));

            var useCase = new ObterBimestreAtualPorTurmaIdUseCase(mockMediator.Object);

            var result = await useCase.Executar(turma.Id);

            Assert.Null(result);
            mockMediator.Verify(m => m.Send(It.IsAny<ObterTurmaPorIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mockMediator.Verify(m => m.Send(It.IsAny<ObterPeriodoEscolarAtualQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Ano_Atual_Periodo_Null_Returns_Null()
        {
            var mockMediator = new Mock<IMediator>();

            var turma = new Turma
            {
                Id = 20,
                AnoLetivo = DateTime.Today.Year 
            };

            mockMediator
                .Setup(m => m.Send(It.IsAny<ObterTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            mockMediator
                .Setup(m => m.Send(It.IsAny<ObterPeriodoEscolarAtualQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((PeriodoEscolar)null);

            var useCase = new ObterBimestreAtualPorTurmaIdUseCase(mockMediator.Object);

            var result = await useCase.Executar(turma.Id);

            Assert.Null(result);
            mockMediator.Verify(m => m.Send(It.IsAny<ObterTurmaPorIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mockMediator.Verify(m => m.Send(It.IsAny<ObterPeriodoEscolarAtualQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Ano_Atual_Periodo_Encontrado_Returns_Bimestre_Dto()
        {
            var mockMediator = new Mock<IMediator>();

            var turma = new Turma
            {
                Id = 30,
                AnoLetivo = DateTime.Today.Year
            };

            var periodo = new PeriodoEscolar
            {
                Id = 55,
                Bimestre = 3,
                PeriodoInicio = DateTime.Today.AddDays(-10),
                PeriodoFim = DateTime.Today.AddDays(10),
                TipoCalendarioId = 1 
            };

            mockMediator
                .Setup(m => m.Send(It.IsAny<ObterTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            mockMediator
                .Setup(m => m.Send(It.IsAny<ObterPeriodoEscolarAtualQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(periodo);

            var useCase = new ObterBimestreAtualPorTurmaIdUseCase(mockMediator.Object);

            var result = await useCase.Executar(turma.Id);

            Assert.NotNull(result);
            Assert.Equal(periodo.Id, result.Id);
            Assert.Equal(periodo.Bimestre, result.Numero);

            mockMediator.Verify(m => m.Send(It.IsAny<ObterTurmaPorIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mockMediator.Verify(m => m.Send(It.Is<ObterPeriodoEscolarAtualQuery>(q => q.TurmaId == turma.Id), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
