using MediatR;
using Moq;
using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.AtribuicaoCJ
{
    public class ObterAnosLetivosAtribuicaoCJUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterAnosLetivosAtribuicaoCJUseCase _useCase;

        public ObterAnosLetivosAtribuicaoCJUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterAnosLetivosAtribuicaoCJUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DeveRetornarAnosLetivosOrdenadosCorretamente()
        {
            // Arrange
            var usuarioRf = "rf123";
            var atribuicoesMock = new List<Dominio.AtribuicaoCJ>
            {
                new Dominio.AtribuicaoCJ { Turma = new Turma { AnoLetivo = 2023 } },
                new Dominio.AtribuicaoCJ { Turma = new Turma { AnoLetivo = 2025 } },
                new Dominio.AtribuicaoCJ { Turma = new Turma { AnoLetivo = 2023 } },
                new Dominio.AtribuicaoCJ { Turma = new Turma { AnoLetivo = 2024 } }
            };

            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoRFQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuarioRf);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAtribuicoesPorTurmaEProfessorQuery>(q =>
                q.UsuarioRf == usuarioRf && q.Substituir == true
            ), It.IsAny<CancellationToken>()))
                .ReturnsAsync(atribuicoesMock);

            // Act
            var result = await _useCase.Executar();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Length);
            Assert.Equal(2025, result[0]);
            Assert.Equal(2024, result[1]);
            Assert.Equal(2023, result[2]);

            _mediatorMock.Verify(m => m.Send(ObterUsuarioLogadoRFQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterAtribuicoesPorTurmaEProfessorQuery>(q =>
                q.UsuarioRf == usuarioRf && q.Substituir == true
            ), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveRetornarNullQuandoNaoHaAtribuicoes()
        {
            // Arrange
            var usuarioRf = "rf123";

            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoRFQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuarioRf);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterAtribuicoesPorTurmaEProfessorQuery>(q =>
                q.UsuarioRf == usuarioRf && q.Substituir == true
            ), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<Dominio.AtribuicaoCJ>());

            // Act
            var result = await _useCase.Executar();

            // Assert
            Assert.Null(result);

            _mediatorMock.Verify(m => m.Send(ObterUsuarioLogadoRFQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterAtribuicoesPorTurmaEProfessorQuery>(q =>
                q.UsuarioRf == usuarioRf && q.Substituir == true
            ), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DevePropagarExcecaoQuandoObterUsuarioLogadoRfFalha()
        {
            // Arrange
            var exceptionMessage = "Erro ao obter RF do usuário logado.";
            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoRFQuery.Instance, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new System.Exception(exceptionMessage));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<System.Exception>(() => _useCase.Executar());
            Assert.Contains(exceptionMessage, ex.Message);

            _mediatorMock.Verify(m => m.Send(ObterUsuarioLogadoRFQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DevePropagarExcecaoQuandoObterAtribuicoesFalha()
        {
            // Arrange
            var usuarioRf = "rf123";
            var exceptionMessage = "Erro ao obter atribuições.";

            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoRFQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuarioRf);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAtribuicoesPorTurmaEProfessorQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new System.Exception(exceptionMessage));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<System.Exception>(() => _useCase.Executar());
            Assert.Contains(exceptionMessage, ex.Message);

            _mediatorMock.Verify(m => m.Send(ObterUsuarioLogadoRFQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterAtribuicoesPorTurmaEProfessorQuery>(q =>
                q.UsuarioRf == usuarioRf && q.Substituir == true
            ), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }
    }
}