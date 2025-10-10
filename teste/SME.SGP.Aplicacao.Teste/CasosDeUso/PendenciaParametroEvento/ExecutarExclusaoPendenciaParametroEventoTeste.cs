using FluentAssertions;
using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PendenciaParametroEvento
{
    public class ExecutarExclusaoPendenciaParametroEventoTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly IExecutarExclusaoPendenciaParametroEvento _useCase;

        public ExecutarExclusaoPendenciaParametroEventoTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExecutarExclusaoPendenciaParametroEvento(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Mensagem_Valida_Deve_Enviar_Comando_E_Retornar_True()
        {
            var command = new VerificaExclusaoPendenciasParametroEventoCommand(99, "UE-12345", TipoEvento.Formacao);
            var mensagemRabbit = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(command)
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<VerificaExclusaoPendenciasParametroEventoCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagemRabbit);

            resultado.Should().BeTrue();

            _mediatorMock.Verify(m => m.Send(
                It.Is<VerificaExclusaoPendenciasParametroEventoCommand>(c =>
                    c.TipoCalendarioId == command.TipoCalendarioId &&
                    c.UeCodigo == command.UeCodigo &&
                    c.TipoEvento == command.TipoEvento),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
