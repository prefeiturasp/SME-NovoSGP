using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PlanoAula
{
    public class ExcluirPlanoAulaPorAulaIdUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ExcluirPlanoAulaPorAulaIdUseCase _useCase;

        public ExcluirPlanoAulaPorAulaIdUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExcluirPlanoAulaPorAulaIdUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Mensagem_Valida_Deve_Enviar_Comando_E_Retornar_True_()
        {
            long aulaId = 12345;
            var filtro = new FiltroIdDto(aulaId);
            var mensagemJson = JsonConvert.SerializeObject(filtro);
            var mensagem = new MensagemRabbit(mensagemJson);

            _mediatorMock.Setup(m => m.Send(It.Is<ExcluirPlanoAulaDaAulaCommand>(c => c.AulaId == aulaId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.Is<ExcluirPlanoAulaDaAulaCommand>(c => c.AulaId == aulaId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Mensagem_Nula_Deve_Lancar_NullReferenceException_()
        {
            MensagemRabbit mensagem = null;

            await Assert.ThrowsAsync<NullReferenceException>(() => _useCase.Executar(mensagem));
        }

        [Fact]
        public async Task Executar_Quando_Conteudo_Mensagem_Nulo_Deve_Lancar_NullReferenceException_()
        {
            var mensagem = new MensagemRabbit(null);

            await Assert.ThrowsAsync<NullReferenceException>(() => _useCase.Executar(mensagem));
        }
    }
}
