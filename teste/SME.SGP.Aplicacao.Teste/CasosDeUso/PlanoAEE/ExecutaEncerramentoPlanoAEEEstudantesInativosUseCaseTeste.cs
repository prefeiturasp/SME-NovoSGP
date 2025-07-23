using MediatR;
using Moq;
using SME.SGP.Aplicacao.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PlanoAEE
{
    public class ExecutaEncerramentoPlanoAEEEstudantesInativosUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly IExecutaEncerramentoPlanoAEEEstudantesInativosUseCase _useCase;

        public ExecutaEncerramentoPlanoAEEEstudantesInativosUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExecutaEncerramentoPlanoAEEEstudantesInativosUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_QuandoMediatorLancaExcecao_DeveRepassarExcecao()
        {
            // Arrange
            var excecaoEsperada = new InvalidOperationException("Erro simulado");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(excecaoEsperada);

            // Act & Assert
            var excecaoLancada = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _useCase.Executar()
            );

            Assert.Equal(excecaoEsperada.Message, excecaoLancada.Message);
        }

        [Fact]
        public void Construtor_QuandoMediatorEhNull_DeveLancarArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new ExecutaEncerramentoPlanoAEEEstudantesInativosUseCase(null)
            );
        }

        [Fact]
        public void UseCase_DeveImplementarInterfaceCorreta()
        {
            // Assert
            Assert.IsAssignableFrom<IExecutaEncerramentoPlanoAEEEstudantesInativosUseCase>(_useCase);
        }

        [Fact]
        public void UseCase_DeveHerdarDeAbstractUseCase()
        {
            // Assert
            Assert.IsAssignableFrom<AbstractUseCase>(_useCase);
        }
    }
}