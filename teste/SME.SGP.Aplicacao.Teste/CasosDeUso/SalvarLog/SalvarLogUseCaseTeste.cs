using MediatR;
using Moq;
using SME.SGP.Dominio.Enumerados;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.SalvarLog
{
    public class SalvarLogUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly SalvarLogUseCase useCase;

        public SalvarLogUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new SalvarLogUseCase(mediatorMock.Object);
        }

        [Fact]
        public void Construtor_Quando_Mediator_Nulo_Deve_Lancar_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>("mediator", () => new SalvarLogUseCase(null));
        }

        [Fact]
        public async Task SalvarInformacao_Quando_Chamado_Deve_Enviar_Comando_Corretamente()
        {
            var mensagem = "Teste de log";
            var contexto = LogContexto.Turma;

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            await useCase.SalvarInformacao(mensagem, contexto);

            mediatorMock.Verify(m => m.Send(
                It.Is<SalvarLogViaRabbitCommand>(cmd =>
                    cmd.Mensagem == mensagem &&
                    cmd.Nivel == LogNivel.Informacao &&
                    cmd.Contexto == contexto &&
                    cmd.Projeto == "SGP"),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
