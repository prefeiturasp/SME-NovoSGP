using FluentAssertions;
using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.CasosDeUso;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Notificacoes
{
    public class NotificacaoFrequenciaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IServicoNotificacaoFrequencia> _servicoNotificacaoFrequenciaMock;
        private readonly INotificacaoFrequenciaUseCase _useCase;

        public NotificacaoFrequenciaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _servicoNotificacaoFrequenciaMock = new Mock<IServicoNotificacaoFrequencia>();
            _useCase = new NotificacaoFrequenciaUseCase(_mediatorMock.Object, _servicoNotificacaoFrequenciaMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Mensagem_Valida_Deve_Acionar_Servico_E_Retornar_True()
        {
            var registroFrequencia = new RegistroFrequencia
            {
                Id = 12345,
                CriadoEm = new DateTime(2025, 10, 9)
            };
            var mensagemRabbit = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(registroFrequencia)
            };

            _servicoNotificacaoFrequenciaMock
                .Setup(s => s.VerificaRegraAlteracaoFrequencia(It.IsAny<long>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(Task.CompletedTask);

            var resultado = await _useCase.Executar(mensagemRabbit);

            resultado.Should().BeTrue();
            _servicoNotificacaoFrequenciaMock.Verify(s => s.VerificaRegraAlteracaoFrequencia(
                registroFrequencia.Id,
                registroFrequencia.CriadoEm,
                It.IsAny<DateTime>()), Times.Once);
        }
    }
}
