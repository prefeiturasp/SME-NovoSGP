using FluentAssertions;
using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PendenciaCalendarioUe
{
    public class ExecutarExclusaoPendenciaDiasLetivosInsuficientesTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly IExecutarExclusaoPendenciaDiasLetivosInsuficientes _useCase;

        public ExecutarExclusaoPendenciaDiasLetivosInsuficientesTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExecutarExclusaoPendenciaDiasLetivosInsuficientes(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Mensagem_Valida_Deve_Enviar_Comando_E_Retornar_True()
        {
            var command = new ExcluirPendenciasDiasLetivosInsuficientesCommand(99, "DRE-COD", "UE-COD");
            var mensagemRabbit = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(command)
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ExcluirPendenciasDiasLetivosInsuficientesCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagemRabbit);

            resultado.Should().BeTrue();

            _mediatorMock.Verify(m => m.Send(
                It.Is<ExcluirPendenciasDiasLetivosInsuficientesCommand>(c =>
                    c.TipoCalendarioId == command.TipoCalendarioId &&
                    c.DreCodigo == command.DreCodigo &&
                    c.UeCodigo == command.UeCodigo),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
