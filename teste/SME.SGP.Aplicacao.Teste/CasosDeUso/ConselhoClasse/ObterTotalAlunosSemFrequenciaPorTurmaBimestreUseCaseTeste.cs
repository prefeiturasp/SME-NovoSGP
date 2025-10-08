using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConselhoClasse
{
    public class ObterTotalAlunosSemFrequenciaPorTurmaBimestreUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterTotalAlunosSemFrequenciaPorTurmaBimestreUseCase _useCase;

        public ObterTotalAlunosSemFrequenciaPorTurmaBimestreUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterTotalAlunosSemFrequenciaPorTurmaBimestreUseCase(_mediatorMock.Object);
        }

        [Fact]
        public void Construtor_Quando_Mediator_Nulo_Deve_Lancar_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterTotalAlunosSemFrequenciaPorTurmaBimestreUseCase(null));
        }

        [Fact]
        public async Task Executar_Quando_Chamado_Deve_Enviar_Query_E_Retornar_Resultado()
        {
            var disciplinaId = "COMP-123";
            var codigoTurma = "TURMA-456";
            var bimestre = 2;
            var dataMatricula = new DateTime(2025, 10, 7);
            var retornoEsperado = new List<int> { 10, 20 };

            _mediatorMock.Setup(m => m.Send(
                It.Is<ObterTotalAlunosSemFrequenciaPorTurmaBimestreQuery>(q =>
                    q.DisciplinaId == disciplinaId &&
                    q.CodigoTurma == codigoTurma &&
                    q.Bimestre == bimestre &&
                    q.DataMatricula == dataMatricula),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(retornoEsperado);

            var resultado = await _useCase.Executar(disciplinaId, codigoTurma, bimestre, dataMatricula);

            Assert.NotNull(resultado);
            Assert.Equal(retornoEsperado, resultado);

            _mediatorMock.Verify(m => m.Send(
                It.Is<ObterTotalAlunosSemFrequenciaPorTurmaBimestreQuery>(q =>
                    q.DisciplinaId == disciplinaId &&
                    q.CodigoTurma == codigoTurma &&
                    q.Bimestre == bimestre &&
                    q.DataMatricula == dataMatricula),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
