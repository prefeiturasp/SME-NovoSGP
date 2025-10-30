using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Notificacao
{
    public class NotificacaoConclusaoEncaminhamentoAEEUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly NotificacaoConclusaoEncaminhamentoAEEUseCase useCase;

        public NotificacaoConclusaoEncaminhamentoAEEUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new NotificacaoConclusaoEncaminhamentoAEEUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_EncaminhamentoMaiorQueZero_Deve_EnviarComandoERetornarTrue_Teste()
        {
            var dto = new NotificacaoEncaminhamentoAEEDto
            {
                EncaminhamentoAEEId = 123,
                UsuarioRF = "12345",
                UsuarioNome = "Teste Usuario"
            };

            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(dto));

            mediatorMock.Setup(x => x.Send(It.IsAny<NotificacaoConclusaoEncaminhamentoAEECommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            mediatorMock.Verify(x => x.Send(It.IsAny<NotificacaoConclusaoEncaminhamentoAEECommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_EncaminhamentoIgualZero_Deve_NaoEnviarComandoERetornarTrue_Teste()
        {
            var dto = new NotificacaoEncaminhamentoAEEDto
            {
                EncaminhamentoAEEId = 0,
                UsuarioRF = "12345",
                UsuarioNome = "Teste Usuario"
            };

            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(dto));

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            mediatorMock.Verify(x => x.Send(It.IsAny<NotificacaoConclusaoEncaminhamentoAEECommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
