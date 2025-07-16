using MediatR;
using Moq;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Fechamento
{
    public class NotificarAlteracaoParecerConclusivoConselhoAgrupadaTurmaUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly NotificarAlteracaoParecerConclusivoConselhoAgrupadaTurmaUseCase useCase;

        public NotificarAlteracaoParecerConclusivoConselhoAgrupadaTurmaUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new NotificarAlteracaoParecerConclusivoConselhoAgrupadaTurmaUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Enviar_Comando_E_Retornar_True()
        {
            mediatorMock
                .Setup(m => m.Send(It.IsAny<NotificarAlteracaoParecerConclusivoConselhoAgrupadaTurmaCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var mensagem = new MensagemRabbit
            {
                Mensagem = null
            };

            var resultado = await useCase.Executar(mensagem);

            mediatorMock.Verify(m => m.Send(It.IsAny<NotificarAlteracaoParecerConclusivoConselhoAgrupadaTurmaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.True(resultado);
        }
    }
}
