using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Aula
{
    public class ExcluirAulaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ExcluirAulaUseCase _useCase;

        public ExcluirAulaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExcluirAulaUseCase(_mediatorMock.Object);
        }

        private SME.SGP.Dominio.Aula CriarAula(long id = 1, string disciplinaId = "123")
        {
            return new SME.SGP.Dominio.Aula
            {
                Id = id,
                DisciplinaId = disciplinaId
            };
        }

        private ComponenteCurricular CriarComponenteCurricular(string nome = "Matemática")
        {
            return new ComponenteCurricular { Descricao = nome };
        }

        [Fact]
        public async Task Executar_DeveLancarExcecao_QuandoAulaNaoForEncontrada()
        {
            // Arrange
            var dto = new ExcluirAulaDto { AulaId = 1, RecorrenciaAula = RecorrenciaAula.AulaUnica };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAulaPorIdQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((SME.SGP.Dominio.Aula)null);

            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(dto));
        }

        [Fact]
        public async Task Executar_DeveChamarExcluirAulaUnica_QuandoRecorrenciaForAulaUnica()
        {
            // Arrange
            var aula = CriarAula();
            var dto = new ExcluirAulaDto { AulaId = aula.Id, RecorrenciaAula = RecorrenciaAula.AulaUnica };
            var usuario = new Usuario();
            var retornoEsperado = new RetornoBaseDto("Aula excluída com sucesso");

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAulaPorIdQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(aula);

            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(usuario);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponenteCurricularPorIdQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new DisciplinaDto { Nome = "Matemática" });

            _mediatorMock.Setup(m => m.Send(It.IsAny<ExcluirAulaUnicaCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(retornoEsperado);

            // Act
            var resultado = await _useCase.Executar(dto);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("Aula excluída com sucesso", resultado.Mensagens[0]);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ExcluirAulaUnicaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_DeveChamarFilaDeExclusao_QuandoRecorrenciaNaoForAulaUnica()
        {
            // Arrange
            var aula = CriarAula();
            var dto = new ExcluirAulaDto { AulaId = aula.Id, RecorrenciaAula = RecorrenciaAula.RepetirBimestreAtual };
            var usuario = new Usuario();
            var componente = CriarComponenteCurricular();

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAulaPorIdQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(aula);

            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(usuario);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponenteCurricularPorIdQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new DisciplinaDto { Nome = "Matemática" });

            _mediatorMock.Setup(m => m.Send(It.IsAny<IncluirFilaExclusaoAulaRecorrenteCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            // Act
            var resultado = await _useCase.Executar(dto);

            // Assert
            Assert.NotNull(resultado);
            Assert.Contains("Serão excluidas aulas recorrentes", resultado.Mensagens[0]);

            _mediatorMock.Verify(m => m.Send(It.IsAny<IncluirFilaExclusaoAulaRecorrenteCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_DeveRegistrarLog_QuandoErroNaFilaDeExclusao()
        {
            // Arrange
            var aula = CriarAula();
            var dto = new ExcluirAulaDto { AulaId = aula.Id, RecorrenciaAula = RecorrenciaAula.RepetirTodosBimestres };
            var usuario = new Usuario();
            var componente = CriarComponenteCurricular();

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAulaPorIdQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(aula);

            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(usuario);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponenteCurricularPorIdQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new DisciplinaDto { Nome = "Matemática" });

            _mediatorMock.Setup(m => m.Send(It.IsAny<IncluirFilaExclusaoAulaRecorrenteCommand>(), It.IsAny<CancellationToken>()))
                         .ThrowsAsync(new Exception("Erro simulado"));

            _mediatorMock.Setup(m => m.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            // Act
            var resultado = await _useCase.Executar(dto);

            // Assert
            Assert.NotNull(resultado);
            Assert.Contains("Ocorreu um erro ao solicitar a exclusão de aulas recorrentes", resultado.Mensagens[0]);

            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
