using FluentAssertions;
using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PendenciaFechamento
{
    public class VerificaPendenciasFechamentoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly IVerificaPendenciasFechamentoUseCase _useCase;

        public VerificaPendenciasFechamentoUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new VerificaPendenciasFechamentoUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Mensagem_Valida_Deve_Enviar_Comando_E_Retornar_True()
        {
            var command = new VerificaPendenciasFechamentoCommand(123, 4, 987);
            var mensagemRabbit = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(command)
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<VerificaPendenciasFechamentoCommand>(), It.IsAny<CancellationToken>()))
                         .Returns(Task.FromResult(Unit.Value));

            var resultado = await _useCase.Executar(mensagemRabbit);

            resultado.Should().BeTrue();

            _mediatorMock.Verify(m => m.Send(
                    It.Is<VerificaPendenciasFechamentoCommand>(c =>
                        c.FechamentoId == command.FechamentoId &&
                        c.Bimestre == command.Bimestre &&
                        c.TurmaId == command.TurmaId),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
