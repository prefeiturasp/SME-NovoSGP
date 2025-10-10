using FluentAssertions;
using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PendenciaAusenciaFechamento
{
    public class ExecutarExclusaoPendenciasAusenciaFechamentoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly IExecutarExclusaoPendenciasAusenciaFechamentoUseCase _useCase;

        public ExecutarExclusaoPendenciasAusenciaFechamentoUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExecutarExclusaoPendenciasAusenciaFechamentoUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Mensagem_Valida_Deve_Enviar_Comando_E_Retornar_True()
        {
            var command = new VerificaExclusaoPendenciasAusenciaFechamentoCommand(123, 4, 987);
            var mensagemRabbit = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(command)
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<VerificaExclusaoPendenciasAusenciaFechamentoCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagemRabbit);

            resultado.Should().BeTrue();

            _mediatorMock.Verify(m => m.Send(
                    It.Is<VerificaExclusaoPendenciasAusenciaFechamentoCommand>(c =>
                        c.DisciplinaId == command.DisciplinaId &&
                        c.PeriodoEscolarId == command.PeriodoEscolarId &&
                        c.TurmaId == command.TurmaId),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
