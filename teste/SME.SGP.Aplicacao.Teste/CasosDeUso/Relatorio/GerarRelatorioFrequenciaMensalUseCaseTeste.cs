using Moq;
using SME.SGP.Infra;
using SME.SGP.Dominio;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using System;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class GerarRelatorioFrequenciaMensalUseCaseTeste
    {
        private readonly Mock<IMediator> _mediator;
        private readonly GerarRelatorioFrequenciaMensalUseCase _useCase;

        public GerarRelatorioFrequenciaMensalUseCaseTeste()
        {
            _mediator = new Mock<IMediator>();
            _useCase = new GerarRelatorioFrequenciaMensalUseCase(_mediator.Object);
        }

        private FiltroRelatorioFrequenciaMensalDto CriarFiltroPadrao()
        {
            return new FiltroRelatorioFrequenciaMensalDto
            {
                AnoLetivo = 2025,
                Modalidade = Modalidade.Fundamental,
                TipoFormatoRelatorio = TipoFormatoRelatorio.Pdf,
                CodigoDre = "111111", 
                CodigoUe = "111111" 
            };
        }

        private Usuario CriarUsuarioPadrao()
        {
            return new Usuario
            {
                CodigoRf = "1111111",
                Nome = "Usuario Teste"
            };
        }

        [Fact]
        public void Deve_Lancar_Excecao_Quando_Mediator_For_Nulo()
        {
            Assert.Throws<ArgumentNullException>(() => new GerarRelatorioFrequenciaMensalUseCase(null));
        }

        [Fact]
        public async Task Deve_Executar_Com_Sucesso_E_Retornar_True()
        {
            var filtro = CriarFiltroPadrao();
            var usuario = CriarUsuarioPadrao();

            _mediator.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);

            _mediator.Setup(m => m.Send(It.IsAny<GerarRelatorioCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await _useCase.Executar(filtro);

            Assert.True(resultado);
        }

        [Fact]
        public async Task Deve_Definir_Usuario_No_Filtro()
        {
            var filtro = CriarFiltroPadrao();
            var usuario = CriarUsuarioPadrao();

            _mediator.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);

            _mediator.Setup(m => m.Send(It.IsAny<GerarRelatorioCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            await _useCase.Executar(filtro);

            Assert.Equal(usuario.Nome, filtro.UsuarioNome);
            Assert.Equal(usuario.CodigoRf, filtro.UsuarioRf);
        }

        [Fact]
        public async Task Deve_Chamar_ObterUsuarioLogadoQuery()
        {
            var filtro = CriarFiltroPadrao();
            var usuario = CriarUsuarioPadrao();

            _mediator.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);

            _mediator.Setup(m => m.Send(It.IsAny<GerarRelatorioCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            await _useCase.Executar(filtro);

            _mediator.Verify(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Chamar_GerarRelatorioCommand_Com_Parametros_Corretos()
        {
            var filtro = CriarFiltroPadrao();
            var usuario = CriarUsuarioPadrao();

            _mediator.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);

            _mediator.Setup(m => m.Send(It.IsAny<GerarRelatorioCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            await _useCase.Executar(filtro);

            _mediator.Verify(m => m.Send(It.Is<GerarRelatorioCommand>(cmd =>
                cmd.TipoRelatorio == TipoRelatorio.FrequenciaMensal &&
                cmd.Filtros == filtro &&
                cmd.RotaRelatorio == RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosFrequenciaMensal
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_False_Quando_GerarRelatorioCommand_Retornar_False()
        {
            var filtro = CriarFiltroPadrao();
            var usuario = CriarUsuarioPadrao();

            _mediator.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);

            _mediator.Setup(m => m.Send(It.IsAny<GerarRelatorioCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var resultado = await _useCase.Executar(filtro);

            Assert.False(resultado);
        }

        [Fact]
        public async Task Deve_Propagar_Excecao_Quando_ObterUsuarioLogadoQuery_Falhar()
        {
            var filtro = CriarFiltroPadrao();

            _mediator.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro ao obter usuário"));

            await Assert.ThrowsAsync<Exception>(() => _useCase.Executar(filtro));
        }

        [Fact]
        public async Task Deve_Propagar_Excecao_Quando_GerarRelatorioCommand_Falhar()
        {
            var filtro = CriarFiltroPadrao();
            var usuario = CriarUsuarioPadrao();

            _mediator.Setup(m => m.Send(ObterUsuarioLogadoQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);

            _mediator.Setup(m => m.Send(It.IsAny<GerarRelatorioCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro ao gerar relatório"));

            await Assert.ThrowsAsync<Exception>(() => _useCase.Executar(filtro));
        }
    }
}