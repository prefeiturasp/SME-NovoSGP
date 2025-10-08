using MediatR;
using Moq;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConselhoClasse
{
    public class ObterTotalAulasNaoLancamNotaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterTotalAulasNaoLancamNotaUseCase _useCase;

        public ObterTotalAulasNaoLancamNotaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterTotalAulasNaoLancamNotaUseCase(_mediatorMock.Object);
        }

        [Fact]
        public void Construtor_Quando_Mediator_Nulo_Deve_Lancar_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterTotalAulasNaoLancamNotaUseCase(null));
        }

        [Fact]
        public async Task Executar_Quando_Chamado_Deve_Enviar_Query_E_Retornar_Resultado()
        {
            var codigoTurma = "123456";
            var bimestre = 2;
            var codigoAluno = "123999";
            var retornoEsperado = new List<TotalAulasNaoLancamNotaDto>
            {
                new TotalAulasNaoLancamNotaDto { DisciplinaId = 1, TotalAulas = "10" }
            };

            _mediatorMock.Setup(m => m.Send(
                It.Is<ObterTotalAulasNaoLancamNotaQuery>(q =>
                    q.CodigoTurma == codigoTurma &&
                    q.Bimestre == bimestre &&
                    q.CodigoAluno == codigoAluno),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(retornoEsperado);

            var resultado = await _useCase.Executar(codigoTurma, bimestre, codigoAluno);

            Assert.NotNull(resultado);
            Assert.Equal(retornoEsperado, resultado);

            _mediatorMock.Verify(m => m.Send(
                It.Is<ObterTotalAulasNaoLancamNotaQuery>(q =>
                    q.CodigoTurma == codigoTurma &&
                    q.Bimestre == bimestre &&
                    q.CodigoAluno == codigoAluno),
                It.IsAny<CancellationToken>()), Moq.Times.Once);
        }
    }
}
