using MediatR;
using Moq;
using SME.SGP.Infra.Dtos.ConselhoClasse;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConselhoClasse
{
    public class ObterTotalAulasSemFrequenciaPorTurmaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterTotalAulasSemFrequenciaPorTurmaUseCase _useCase;

        public ObterTotalAulasSemFrequenciaPorTurmaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterTotalAulasSemFrequenciaPorTurmaUseCase(_mediatorMock.Object);
        }

        [Fact]
        public void Construtor_Quando_Mediator_Nulo_Deve_Lancar_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterTotalAulasSemFrequenciaPorTurmaUseCase(null));
        }

        [Fact]
        public async Task Executar_Quando_Chamado_Deve_Enviar_Query_E_Retornar_Resultado()
        {
            var disciplinaId = "COMP-123";
            var codigoTurma = "TURMA-456";
            var retornoEsperado = new List<TotalAulasPorAlunoTurmaDto>
            {
                new TotalAulasPorAlunoTurmaDto { DisciplinaId = disciplinaId, TotalAulas = "50" }
            };

            _mediatorMock.Setup(m => m.Send(
                It.Is<ObterTotalAulasSemFrequenciaPorTurmaQuery>(q =>
                    q.DisciplinaId == disciplinaId &&
                    q.CodigoTurma == codigoTurma),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(retornoEsperado);

            var resultado = await _useCase.Executar(disciplinaId, codigoTurma);

            Assert.NotNull(resultado);
            Assert.Equal(retornoEsperado, resultado);

            _mediatorMock.Verify(m => m.Send(
                It.Is<ObterTotalAulasSemFrequenciaPorTurmaQuery>(q =>
                    q.DisciplinaId == disciplinaId &&
                    q.CodigoTurma == codigoTurma),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
