using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Relatorio
{
    public class GerarRelatorioFrequenciaMensalUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly GerarRelatorioFrequenciaMensalUseCase _useCase;

        public GerarRelatorioFrequenciaMensalUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new GerarRelatorioFrequenciaMensalUseCase(_mediatorMock.Object);
        }

        [Fact]
        public void DeveLancarExcecao_QuandoMediatorForNulo()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new GerarRelatorioFrequenciaMensalUseCase(null));
        }

        [Fact]
        public async Task DevePreencherUsuarioNoFiltro_E_ChamarGerarRelatorio()
        {
            // Arrange
            var filtro = new FiltroRelatorioFrequenciaMensalDto
            {
                CodigoDre = "108300",
                CodigoUe = "012345"
            };
            var usuario = CriarUsuarioMock();
            FiltroRelatorioFrequenciaMensalDto filtroCapturado = null;

            ConfigurarMocks(usuario);

            _mediatorMock.Setup(m => m.Send(It.IsAny<GerarRelatorioCommand>(), It.IsAny<CancellationToken>()))
                         .Callback<IRequest<bool>, CancellationToken>((cmd, ct) =>
                         {
                             var comando = Assert.IsType<GerarRelatorioCommand>(cmd);
                             filtroCapturado = Assert.IsType<FiltroRelatorioFrequenciaMensalDto>(comando.Filtros);
                         })
                         .ReturnsAsync(true);

            // Act
            await _useCase.Executar(filtro);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.IsAny<GerarRelatorioCommand>(), default), Times.Once);
            Assert.NotNull(filtroCapturado);
            Assert.Equal(usuario.Nome, filtroCapturado.UsuarioNome);
            Assert.Equal(usuario.CodigoRf, filtroCapturado.UsuarioRf);
        }

        [Theory]
        [InlineData("-99")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task DeveUsarRotaTodos_QuandoCodigoDreForInvalido(string codigoDre)
        {
            // Arrange
            var filtro = new FiltroRelatorioFrequenciaMensalDto { CodigoDre = codigoDre, CodigoUe = "012345" };
            var usuario = CriarUsuarioMock();
            GerarRelatorioCommand comandoCapturado = null;

            ConfigurarMocks(usuario);
            ConfigurarCallbackComando(cmd => comandoCapturado = cmd);

            // Act
            await _useCase.Executar(filtro);

            // Assert
            Assert.NotNull(comandoCapturado);
            Assert.Equal(RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosFrequenciaMensalTodosDreUe, comandoCapturado.RotaRelatorio);
        }

        [Theory]
        [InlineData("-99")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task DeveUsarRotaTodos_QuandoCodigoUeForInvalido(string codigoUe)
        {
            // Arrange
            var filtro = new FiltroRelatorioFrequenciaMensalDto { CodigoDre = "108300", CodigoUe = codigoUe };
            var usuario = CriarUsuarioMock();
            GerarRelatorioCommand comandoCapturado = null;

            ConfigurarMocks(usuario);
            ConfigurarCallbackComando(cmd => comandoCapturado = cmd);

            // Act
            await _useCase.Executar(filtro);

            // Assert
            Assert.NotNull(comandoCapturado);
            Assert.Equal(RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosFrequenciaMensalTodosDreUe, comandoCapturado.RotaRelatorio);
        }

        [Fact]
        public async Task DeveUsarRotaEspecifica_QuandoDreEUeForemValidos()
        {
            // Arrange
            var filtro = new FiltroRelatorioFrequenciaMensalDto
            {
                CodigoDre = "108300",
                CodigoUe = "012345"
            };
            var usuario = CriarUsuarioMock();
            GerarRelatorioCommand comandoCapturado = null;

            ConfigurarMocks(usuario);
            ConfigurarCallbackComando(cmd => comandoCapturado = cmd);

            // Act
            await _useCase.Executar(filtro);

            // Assert
            Assert.NotNull(comandoCapturado);
            Assert.Equal(RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosFrequenciaMensal, comandoCapturado.RotaRelatorio);
        }

        private void ConfigurarMocks(Usuario usuario)
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), default))
                         .ReturnsAsync(usuario);
        }

        private void ConfigurarCallbackComando(Action<GerarRelatorioCommand> callback)
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<GerarRelatorioCommand>(), It.IsAny<CancellationToken>()))
                         .Callback<IRequest<bool>, CancellationToken>((cmd, ct) =>
                         {
                             var comando = Assert.IsType<GerarRelatorioCommand>(cmd);
                             callback(comando);
                         })
                         .ReturnsAsync(true);
        }

        private Usuario CriarUsuarioMock()
        {
            return new Usuario
            {
                Id = 99,
                Nome = "Usuário Teste Relatório Mensal",
                CodigoRf = "rf123456",
                PerfilAtual = Guid.NewGuid()
            };
        }
    }
}