using MediatR;
using Moq;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.WorkflowAprovacao
{
    public class ExcluirWorkflowAprovacaoPorIdUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ExcluirWorkflowAprovacaoPorIdUseCase useCase;

        public ExcluirWorkflowAprovacaoPorIdUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ExcluirWorkflowAprovacaoPorIdUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Mensagem_Valida_Deve_Teste_Teste()
        {
            var filtro = new FiltroIdDto(10);
            var mensagem = new MensagemRabbit(Newtonsoft.Json.JsonConvert.SerializeObject(filtro));

            mediatorMock.Setup(x => x.Send(It.IsAny<ExcluirWorkflowCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            mediatorMock.Verify(x => x.Send(It.Is<ExcluirWorkflowCommand>(c => c.WorkflowId == 10), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
