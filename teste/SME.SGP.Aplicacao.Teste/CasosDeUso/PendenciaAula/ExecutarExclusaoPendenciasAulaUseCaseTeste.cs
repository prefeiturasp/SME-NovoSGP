using FluentAssertions;
using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PendenciaAula
{
    public class ExecutarExclusaoPendenciasAulaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly IExecutarExclusaoPendenciasAulaUseCase _useCase;

        public ExecutarExclusaoPendenciasAulaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExecutarExclusaoPendenciasAulaUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Mensagem_Valida_Deve_Enviar_Comando_E_Retornar_True()
        {
            var filtro = new FiltroIdDto(98765);
            var mensagemRabbit = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(filtro)
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ExcluirTodasPendenciasAulaCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagemRabbit);

            resultado.Should().BeTrue();

            _mediatorMock.Verify(m => m.Send(
                    It.Is<ExcluirTodasPendenciasAulaCommand>(c => c.AulaId == filtro.Id),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
