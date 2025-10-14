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
    public class ObterTotalCompensacoesComponenteNaoLancaNotaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterTotalCompensacoesComponenteNaoLancaNotaUseCase _useCase;

        public ObterTotalCompensacoesComponenteNaoLancaNotaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterTotalCompensacoesComponenteNaoLancaNotaUseCase(_mediatorMock.Object);
        }

        [Fact]
        public void Construtor_Quando_Mediator_Nulo_Deve_Lancar_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterTotalCompensacoesComponenteNaoLancaNotaUseCase(null));
        }

        [Fact]
        public async Task Executar_Quando_Chamado_Deve_Enviar_Query_E_Retornar_Resultado()
        {
            var codigoTurma = "456789";
            var bimestre = 2;
            var retornoEsperado = new List<TotalCompensacoesComponenteNaoLancaNotaDto>
            {
                new TotalCompensacoesComponenteNaoLancaNotaDto { DisciplinaId = 1, TotalCompensacoes = "10" }
            };

            _mediatorMock.Setup(m => m.Send(
                It.Is<ObterTotalCompensacoesComponenteNaoLancaNotaQuery>(q =>
                    q.CodigoTurma == codigoTurma &&
                    q.Bimestre == bimestre),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(retornoEsperado);

            var resultado = await _useCase.Executar(codigoTurma, bimestre);

            Assert.NotNull(resultado);
            Assert.Equal(retornoEsperado, resultado);

            _mediatorMock.Verify(m => m.Send(
                It.Is<ObterTotalCompensacoesComponenteNaoLancaNotaQuery>(q =>
                    q.CodigoTurma == codigoTurma &&
                    q.Bimestre == bimestre),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
