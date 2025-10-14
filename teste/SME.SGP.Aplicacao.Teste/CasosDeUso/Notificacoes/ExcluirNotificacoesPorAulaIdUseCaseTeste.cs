using FluentAssertions;
using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.CasosDeUso;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Notificacoes
{
    public class ExcluirNotificacoesPorAulaIdUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly IExcluirNotificacoesPorAulaIdUseCase _useCase;

        public ExcluirNotificacoesPorAulaIdUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExcluirNotificacoesPorAulaIdUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Mensagem_Valida_Deve_Enviar_Comando_E_Retornar_True()
        {
            var filtro = new FiltroIdDto(556677);
            var mensagemRabbit = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(filtro)
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ExcluirNotificacoesDaAulaCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagemRabbit);

            resultado.Should().BeTrue();

            _mediatorMock.Verify(m => m.Send(
                    It.Is<ExcluirNotificacoesDaAulaCommand>(c => c.AulaId == filtro.Id),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
