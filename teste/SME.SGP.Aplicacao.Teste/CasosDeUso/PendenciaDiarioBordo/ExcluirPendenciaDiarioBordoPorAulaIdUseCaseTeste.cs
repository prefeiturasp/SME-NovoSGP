using FluentAssertions;
using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PendenciaDiarioBordo
{
    public class ExcluirPendenciaDiarioBordoPorAulaIdUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly IExcluirPendenciaDiarioBordoPorAulaIdUseCase _useCase;

        public ExcluirPendenciaDiarioBordoPorAulaIdUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExcluirPendenciaDiarioBordoPorAulaIdUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Mensagem_Valida_Deve_Enviar_Comando_E_Retornar_True()
        {
            long id = 6789;
            var filtro = new FiltroIdDto(id);
            var mensagemRabbit = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(filtro)
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ExcluirPendenciaDiarioPorAulaIdCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagemRabbit);

            resultado.Should().BeTrue();

            _mediatorMock.Verify(m => m.Send(
                    It.Is<ExcluirPendenciaDiarioPorAulaIdCommand>(c => c.AulaId == id),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
