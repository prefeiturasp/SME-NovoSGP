using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.AnotacaoFrequenciaAluno
{
    public class ExcluirAnotacoesFrequenciaPorAulaIdUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ExcluirAnotacoesFrequenciaPorAulaIdUseCase _useCase;

        public ExcluirAnotacoesFrequenciaPorAulaIdUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExcluirAnotacoesFrequenciaPorAulaIdUseCase(_mediatorMock.Object);
        }

        private MensagemRabbit CriarMensagemRabbit(long aulaId)
        {
            var filtro = new FiltroIdDto(aulaId);
            var jsonFiltro = JsonConvert.SerializeObject(filtro);
            return new MensagemRabbit(jsonFiltro, Guid.NewGuid(), "Nome", "RF", null);
        }

        [Fact]
        public async Task Executar_Quando_Mensagem_Valida_Deve_Enviar_Comando_E_Retornar_True()
        {
            long aulaId = 123;
            var mensagem = CriarMensagemRabbit(aulaId);

            _mediatorMock.Setup(m => m.Send(It.Is<ExcluirAnotacoesFrequencciaDaAulaCommand>(c => c.AulaId == aulaId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.Is<ExcluirAnotacoesFrequencciaDaAulaCommand>(c => c.AulaId == aulaId), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
