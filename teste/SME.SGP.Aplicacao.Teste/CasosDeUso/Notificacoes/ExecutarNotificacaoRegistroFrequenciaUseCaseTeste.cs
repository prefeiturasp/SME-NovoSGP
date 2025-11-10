using FluentAssertions;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Notificacoes
{
    public class ExecutarNotificacaoRegistroFrequenciaUseCaseTeste
    {
        private readonly Mock<IServicoNotificacaoFrequencia> _servicoMock;
        private readonly IExecutarNotificacaoRegistroFrequenciaUseCase _useCase;

        public ExecutarNotificacaoRegistroFrequenciaUseCaseTeste()
        {
            _servicoMock = new Mock<IServicoNotificacaoFrequencia>();
            _useCase = new ExecutarNotificacaoRegistroFrequenciaUseCase(_servicoMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Chamado_Deve_Acionar_Servico_E_Retornar_True()
        {
            _servicoMock.Setup(s => s.ExecutaNotificacaoRegistroFrequencia()).Returns(Task.CompletedTask);

            var resultado = await _useCase.Executar(new MensagemRabbit());

            resultado.Should().BeTrue();
            _servicoMock.Verify(s => s.ExecutaNotificacaoRegistroFrequencia(), Times.Once);
        }
    }
}
