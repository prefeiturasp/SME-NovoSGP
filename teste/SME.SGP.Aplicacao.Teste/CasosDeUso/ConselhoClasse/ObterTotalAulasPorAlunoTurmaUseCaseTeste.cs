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
    public class ObterTotalAulasPorAlunoTurmaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterTotalAulasPorAlunoTurmaUseCase _useCase;

        public ObterTotalAulasPorAlunoTurmaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterTotalAulasPorAlunoTurmaUseCase(_mediatorMock.Object);
        }

        [Fact]
        public void Construtor_Quando_Mediator_Nulo_Deve_Lancar_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterTotalAulasPorAlunoTurmaUseCase(null));
        }

        [Fact]
        public async Task Executar_Quando_Chamado_Deve_Enviar_Query_E_Retornar_Resultado()
        {
            var codigoAluno = "123";
            var codigoTurma = "456";
            var retornoEsperado = new List<TotalAulasPorAlunoTurmaDto>
            {
                new TotalAulasPorAlunoTurmaDto { CodigoAluno = codigoAluno, DisciplinaId = "1111", TotalAulas = "50" }
            };

            _mediatorMock.Setup(m => m.Send(
                It.Is<ObterTotalAulasPorAlunoTurmaQuery>(q =>
                    q.DisciplinaId == codigoAluno &&
                    q.CodigoTurma == codigoTurma),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(retornoEsperado);

            var resultado = await _useCase.Executar(codigoAluno, codigoTurma);

            Assert.NotNull(resultado);
            Assert.Equal(retornoEsperado, resultado);

            _mediatorMock.Verify(m => m.Send(
                It.Is<ObterTotalAulasPorAlunoTurmaQuery>(q =>
                    q.DisciplinaId == codigoAluno &&
                    q.CodigoTurma == codigoTurma),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
