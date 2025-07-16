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
    public class AlterarAulaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly AlterarAulaUseCase _useCase;

        public AlterarAulaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new AlterarAulaUseCase(_mediatorMock.Object);
        }

        private static PersistirAulaDto CriarDto(RecorrenciaAula recorrencia)
        {
            return new PersistirAulaDto
            {
                Id = 1,
                DataAula = DateTime.Today,
                CodigoComponenteCurricular = 123,
                NomeComponenteCurricular = "Matemática",
                TipoAula = TipoAula.Normal,
                TipoCalendarioId = 1,
                CodigoTurma = "TURMA1",
                CodigoUe = "UE1",
                Quantidade = 2,
                RecorrenciaAula = recorrencia,
                EhRegencia = false,
            };
        }

        [Fact]
        public async Task Executar_DeveChamarAlterarAulaUnica_QuandoRecorrenciaForAulaUnica()
        {
            // Arrange
            var dto = CriarDto(RecorrenciaAula.AulaUnica);
            var usuario = new Usuario();

            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(usuario);

            var retornoEsperado = new RetornoBaseDto("Aula alterada com sucesso");

            _mediatorMock.Setup(m => m.Send(It.IsAny<AlterarAulaUnicaCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(retornoEsperado);

            // Act
            var resultado = await _useCase.Executar(dto);

            // Assert
            Assert.NotNull(resultado);
            Assert.Contains("Aula alterada com sucesso", resultado.Mensagens);
            _mediatorMock.Verify(m => m.Send(It.IsAny<AlterarAulaUnicaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_DeveChamarIncluirFilaRecorrente_QuandoRecorrente()
        {
            // Arrange
            var dto = CriarDto(RecorrenciaAula.RepetirBimestreAtual);
            var usuario = new Usuario();

            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(usuario);

            _mediatorMock.Setup(m => m.Send(It.IsAny<IncluirFilaAlteracaoAulaRecorrenteCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            // Act
            var resultado = await _useCase.Executar(dto);

            // Assert
            Assert.NotNull(resultado);
            Assert.Contains("Serão alteradas aulas recorrentes", resultado.Mensagens[0]);

            _mediatorMock.Verify(m => m.Send(It.IsAny<IncluirFilaAlteracaoAulaRecorrenteCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_DeveRegistrarLog_QuandoOcorrerErroNaFilaRecorrente()
        {
            // Arrange
            var dto = CriarDto(RecorrenciaAula.RepetirTodosBimestres);
            var usuario = new Usuario();

            _mediatorMock.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(usuario);

            _mediatorMock.Setup(m => m.Send(It.IsAny<IncluirFilaAlteracaoAulaRecorrenteCommand>(), It.IsAny<CancellationToken>()))
                         .ThrowsAsync(new Exception("Erro simulado"));

            _mediatorMock.Setup(m => m.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            // Act
            var resultado = await _useCase.Executar(dto);

            // Assert
            Assert.NotNull(resultado);
            Assert.Contains("Ocorreu um erro ao solicitar a alteração", resultado.Mensagens[0]);

            _mediatorMock.Verify(m => m.Send(It.IsAny<IncluirFilaAlteracaoAulaRecorrenteCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
