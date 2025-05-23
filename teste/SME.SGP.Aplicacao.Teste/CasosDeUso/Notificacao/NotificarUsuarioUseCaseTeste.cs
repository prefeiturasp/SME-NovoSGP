using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Notificacao
{
    public class NotificarUsuarioUseCaseTeste
    {
        [Fact]
        public async Task Deve_Enviar_Notificacao_Rabbit_Para_Usuario()
        {
            var mediatorMock = new Mock<IMediator>();
            var useCase = new NotificarUsuarioUseCase(mediatorMock.Object);

            var command = new NotificarUsuarioCommand(
                "Titulo",
                "Mensagem",
                "8168822",
                NotificacaoCategoria.Aviso,
                NotificacaoTipo.PlanoDeAula,
                "2345",
                "teste",
                "teste"
            );

            var mensagemRabbit = new MensagemRabbit();
            mensagemRabbit.Mensagem = JsonConvert.SerializeObject(command); 

            mediatorMock
                .Setup(m => m.Send(It.IsAny<NotificarUsuarioCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1L);

            var resultado = await useCase.Executar(mensagemRabbit);

            mediatorMock.Verify(m => m.Send(
                It.Is<NotificarUsuarioCommand>(c =>
                    c.Titulo == command.Titulo &&
                    c.Mensagem == command.Mensagem &&
                    c.NomeUsuario == command.NomeUsuario
                ),
                It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(resultado);
        }
    }
}