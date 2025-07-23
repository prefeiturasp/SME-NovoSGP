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
    public class EnviarNotificacaoEncerramentoPlanoAEEUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly EnviarNotificacaoEncerramentoPlanoAEEUseCase useCase;
        private readonly long planoAEEId;

        public EnviarNotificacaoEncerramentoPlanoAEEUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new EnviarNotificacaoEncerramentoPlanoAEEUseCase(mediator.Object);
            planoAEEId = 1234;
        }

        [Fact]
        public async Task Deve_Retornar_True_Quando_Notificacao_For_Enviada_Com_Sucesso()
        {
            // Arrange
            var command = new NotificarEncerramentoPlanoAEECommand(planoAEEId, GetUsuarioTeste());
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(command) };

            mediator.Setup(m => m.Send(It.IsAny<NotificarEncerramentoPlanoAEECommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var resultado = await useCase.Executar(mensagem);

            // Assert
            Assert.True(resultado);
        }

        [Fact]
        public async Task Deve_Retornar_False_Quando_Notificacao_Nao_For_Enviada()
        {
            // Arrange
            var command = new NotificarEncerramentoPlanoAEECommand(planoAEEId, GetUsuarioTeste());
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(command) };

            mediator.Setup(m => m.Send(It.IsAny<NotificarEncerramentoPlanoAEECommand>(),
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
            var command = new NotificarEncerramentoPlanoAEECommand(planoAEEId, GetUsuarioTeste());
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(command) };

            mediator.Setup(m => m.Send(It.IsAny<NotificarEncerramentoPlanoAEECommand>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro ao enviar notificação"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => useCase.Executar(mensagem));
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Mensagem_For_Invalida()
        {
            // Arrange
            var mensagem = new MensagemRabbit { Mensagem = "json inválido" };

            // Act & Assert
            await Assert.ThrowsAsync<Newtonsoft.Json.JsonReaderException>(() => useCase.Executar(mensagem));
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Command_For_Mensagem_Invalida()
        {
            // Arrange
            var mensagem = new MensagemRabbit { Mensagem = "/'null" };

            // Act & Assert
            await Assert.ThrowsAsync<Newtonsoft.Json.JsonReaderException>(() => useCase.Executar(mensagem));
        }

        private SME.SGP.Dominio.Usuario GetUsuarioTeste()
        {
            return new Dominio.Usuario { Nome = "Joao", CodigoRf = "1234" };
        }
    }
}
