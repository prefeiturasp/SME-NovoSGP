using MediatR;
using Moq;
using SME.SGP.Aplicacao.Queries.Informes.VerificaSeExisteNotificacaoInformePorIdUsuarioRf;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Informes
{
    public class ExecutarNotificacaoInformativoUsuarioUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<IUnitOfWork> unitOfWorkMock;
        private readonly ExecutarNotificacaoInformativoUsuarioUseCase useCase;

        public ExecutarNotificacaoInformativoUsuarioUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            unitOfWorkMock = new Mock<IUnitOfWork>();
            useCase = new ExecutarNotificacaoInformativoUsuarioUseCase(mediatorMock.Object, unitOfWorkMock.Object);
        }

        [Fact]
        public void Construtor_Deve_Instanciar_Corretamente()
        {
            var resultado = new ExecutarNotificacaoInformativoUsuarioUseCase(mediatorMock.Object, unitOfWorkMock.Object);

            Assert.NotNull(resultado);
        }

        [Fact]
        public void Construtor_Deve_Lancar_Excecao_Quando_UnitOfWork_For_Nulo()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new ExecutarNotificacaoInformativoUsuarioUseCase(mediatorMock.Object, null));

            Assert.Equal("unitOfWork", exception.ParamName);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_False_Quando_Informe_For_Excluido()
        {
            var mensagemRabbit = CriarMensagemRabbitMock();
            mediatorMock.Setup(m => m.Send(It.IsAny<InformeFoiExcluidoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagemRabbit);

            Assert.False(resultado);
            unitOfWorkMock.Verify(u => u.IniciarTransacao(), Times.Never);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_False_Quando_Notificacao_Ja_Existir()
        {
            var mensagemRabbit = CriarMensagemRabbitMock();
            mediatorMock.Setup(m => m.Send(It.IsAny<InformeFoiExcluidoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            mediatorMock.Setup(m => m.Send(It.IsAny<VerificaSeExisteNotificacaoInformePorIdUsuarioRfQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagemRabbit);

            Assert.False(resultado);
            unitOfWorkMock.Verify(u => u.IniciarTransacao(), Times.Never);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_True_Quando_Notificacao_For_Criada_Com_Sucesso()
        {
            var mensagemRabbit = CriarMensagemRabbitMock();
            var usuarioId = 123L;
            var notificacaoId = 456L;

            ConfigurarMediatorParaSucesso(usuarioId, notificacaoId);

            var resultado = await useCase.Executar(mensagemRabbit);

            Assert.True(resultado);
            unitOfWorkMock.Verify(u => u.IniciarTransacao(), Times.Once);
            unitOfWorkMock.Verify(u => u.PersistirTransacao(), Times.Once);
            unitOfWorkMock.Verify(u => u.Rollback(), Times.Never);

            mediatorMock.Verify(m => m.Send(It.IsAny<ObterUsuarioIdPorRfOuCriaQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<NotificarUsuarioCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<SalvarInformativoNotificacaoCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Deve_Executar_Rollback_Quando_Ocorrer_Excecao()
        {
            var mensagemRabbit = CriarMensagemRabbitMock();

            mediatorMock.Setup(m => m.Send(It.IsAny<InformeFoiExcluidoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            mediatorMock.Setup(m => m.Send(It.IsAny<VerificaSeExisteNotificacaoInformePorIdUsuarioRfQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioIdPorRfOuCriaQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro simulado"));

            await Assert.ThrowsAsync<Exception>(() => useCase.Executar(mensagemRabbit));

            unitOfWorkMock.Verify(u => u.IniciarTransacao(), Times.Once);
            unitOfWorkMock.Verify(u => u.Rollback(), Times.Once);
            unitOfWorkMock.Verify(u => u.PersistirTransacao(), Times.Never);
        }

        [Fact]
        public async Task Executar_Deve_Gerar_Codigo_Notificacao_Correto()
        {
            var mensagemRabbit = CriarMensagemRabbitMock();
            var informativoId = 100L;
            var usuarioId = 123L;
            var codigoEsperado = 100000123L;
            long codigoCapturado = 0;

            mediatorMock.Setup(m => m.Send(It.IsAny<InformeFoiExcluidoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            mediatorMock.Setup(m => m.Send(It.IsAny<VerificaSeExisteNotificacaoInformePorIdUsuarioRfQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioIdPorRfOuCriaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuarioId);
            mediatorMock.Setup(m => m.Send(It.IsAny<NotificarUsuarioCommand>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<long>, CancellationToken>((cmd, token) =>
                {
                    var command = cmd as NotificarUsuarioCommand;
                    codigoCapturado = command.Codigo;
                })
                .ReturnsAsync(456L);

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarInformativoNotificacaoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(informativoId);

            await useCase.Executar(mensagemRabbit);

            Assert.Equal(codigoEsperado, codigoCapturado);
        }

        [Fact]
        public async Task Executar_Deve_Passar_Parametros_Corretos_Para_Notificar_Usuario_Command()
        {
            var filtro = new NotificacaoInformativoUsuarioFiltro
            {
                InformativoId = 100,
                UsuarioRf = "1234567",
                Titulo = "Título Teste",
                Mensagem = "Mensagem Teste",
                DreCodigo = "DRE01",
                UeCodigo = "UE01"
            };
            var mensagemRabbit = CriarMensagemRabbitMock(filtro);
            var usuarioId = 123L;
            NotificarUsuarioCommand commandCapturado = null;

            ConfigurarMediatorParaSucesso(usuarioId, 456L);

            mediatorMock.Setup(m => m.Send(It.IsAny<NotificarUsuarioCommand>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<long>, CancellationToken>((cmd, token) =>
                {
                    commandCapturado = cmd as NotificarUsuarioCommand;
                })
                .ReturnsAsync(456L);

            await useCase.Executar(mensagemRabbit);

            Assert.NotNull(commandCapturado);
            Assert.Equal(filtro.Titulo, commandCapturado.Titulo);
            Assert.Equal(filtro.Mensagem, commandCapturado.Mensagem);
            Assert.Equal(filtro.UsuarioRf, commandCapturado.UsuarioRf);
            Assert.Equal(NotificacaoCategoria.Informe, commandCapturado.Categoria);
            Assert.Equal(NotificacaoTipo.Customizado, commandCapturado.Tipo);
            Assert.Equal(filtro.DreCodigo, commandCapturado.DreCodigo);
            Assert.Equal(filtro.UeCodigo, commandCapturado.UeCodigo);
            Assert.Equal(string.Empty, commandCapturado.TurmaCodigo);
            Assert.Equal(0, commandCapturado.Ano);
            Assert.Equal(usuarioId, commandCapturado.UsuarioId);
        }

        private MensagemRabbit CriarMensagemRabbitMock(NotificacaoInformativoUsuarioFiltro filtro = null)
        {
            filtro ??= new NotificacaoInformativoUsuarioFiltro
            {
                InformativoId = 100,
                UsuarioRf = "1234567",
                Titulo = "Título",
                Mensagem = "Mensagem",
                DreCodigo = "DRE",
                UeCodigo = "UE"
            };

            return new MensagemRabbit(JsonSerializer.Serialize(filtro));
        }

        private void ConfigurarMediatorParaSucesso(long usuarioId, long notificacaoId)
        {
            mediatorMock.Setup(m => m.Send(It.IsAny<InformeFoiExcluidoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            mediatorMock.Setup(m => m.Send(It.IsAny<VerificaSeExisteNotificacaoInformePorIdUsuarioRfQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioIdPorRfOuCriaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuarioId);
            mediatorMock.Setup(m => m.Send(It.IsAny<NotificarUsuarioCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(notificacaoId);
            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarInformativoNotificacaoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);
        }
    }
}
