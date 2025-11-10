using FluentAssertions;
using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PendenciaDevolutiva
{
    public class ExecutarExclusaoPendenciasDevolutivaUseCaseTeste
    {

        private readonly Mock<IMediator> _mediatorMock;
        private readonly IExecutarExclusaoPendenciasDevolutivaUseCase _useCase;

        public ExecutarExclusaoPendenciasDevolutivaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExecutarExclusaoPendenciasDevolutivaUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Mensagem_Valida_Deve_Enviar_Comando_E_Retornar_True()
        {
            var filtro = new FiltroExclusaoPendenciasDevolutivaDto
            {
                TurmaId = 123,
                ComponenteId = 456
            };

            var mensagemRabbit = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(filtro)
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ExcluirPendenciasDevolutivaPorTurmaEComponenteCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagemRabbit);

            resultado.Should().BeTrue();

            _mediatorMock.Verify(m => m.Send(
                    It.Is<ExcluirPendenciasDevolutivaPorTurmaEComponenteCommand>(c =>
                        c.TurmaId == filtro.TurmaId &&
                        c.ComponenteId == filtro.ComponenteId),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
