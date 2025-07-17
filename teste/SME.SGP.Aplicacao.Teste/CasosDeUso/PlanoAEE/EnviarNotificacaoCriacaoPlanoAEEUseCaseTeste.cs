using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PlanoAEE
{
    public class EnviarNotificacaoCriacaoPlanoAEEUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly EnviarNotificacaoCriacaoPlanoAEEUseCase useCase;

        public EnviarNotificacaoCriacaoPlanoAEEUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new EnviarNotificacaoCriacaoPlanoAEEUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Retornar_True_Quando_Notificacao_For_Enviada_Com_Sucesso()
        {
            // Arrange
            var usuario = new Dominio.Usuario { Nome = "teste" };
            var command = new EnviarNotificacaoCriacaoPlanoAEECommand(planoAEEId: 1, usuario);
            var mensagem = new MensagemRabbit { Mensagem = Newtonsoft.Json.JsonConvert.SerializeObject(command) };

            // Configuração mais flexível do mock
            mediator.Setup(m => m.Send(It.Is<EnviarNotificacaoCriacaoPlanoAEECommand>(
                    c => c.PlanoAEEId == command.PlanoAEEId &&
                         c.Usuario.Nome == command.Usuario.Nome),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var resultado = await useCase.Executar(mensagem);

            // Assert
            Assert.True(resultado);
            mediator.Verify(m => m.Send(It.Is<EnviarNotificacaoCriacaoPlanoAEECommand>(
                c => c.PlanoAEEId == command.PlanoAEEId &&
                     c.Usuario.Nome == command.Usuario.Nome),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_False_Quando_Notificacao_Nao_For_Enviada()
        {
            // Arrange
            var command = new EnviarNotificacaoCriacaoPlanoAEECommand(planoAEEId: 1, new Dominio.Usuario { Nome = "teste" });

            var mensagem = new MensagemRabbit { Mensagem = Newtonsoft.Json.JsonConvert.SerializeObject(command) };

            mediator.Setup(m => m.Send(It.IsAny<EnviarNotificacaoCriacaoPlanoAEECommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var resultado = await useCase.Executar(mensagem);

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public async Task Deve_Tratar_Excecao_Do_Command()
        {
            // Arrange
            var command = new EnviarNotificacaoCriacaoPlanoAEECommand(planoAEEId: 1, new Dominio.Usuario { Nome = "teste" });

            var mensagem = new MensagemRabbit { Mensagem = Newtonsoft.Json.JsonConvert.SerializeObject(command) };

            mediator.Setup(m => m.Send(It.IsAny<EnviarNotificacaoCriacaoPlanoAEECommand>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro ao enviar notificação"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => useCase.Executar(mensagem));
        }

        [Fact]
        public async Task Deve_Lancar_JsonException_Quando_Mensagem_For_Invalida()
        {
            // Arrange
            var mensagem = new MensagemRabbit { Mensagem = "json inválido" };

            // Act & Assert
            await Assert.ThrowsAsync<JsonReaderException>(() => useCase.Executar(mensagem));
        }

        [Fact]
        public async Task Nao_Deve_Lancar_Excecao_Quando_Command_For_Nulo()
        {
            // Arrange
            var mensagem = new MensagemRabbit { Mensagem = "null" };

            mediator.Setup(m => m.Send(It.IsAny<EnviarNotificacaoCriacaoPlanoAEECommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var resultado = await useCase.Executar(mensagem);

            // Assert
            // Verifica que o fluxo continua mesmo com comando nulo
            Assert.True(resultado);
        }
    }
}

