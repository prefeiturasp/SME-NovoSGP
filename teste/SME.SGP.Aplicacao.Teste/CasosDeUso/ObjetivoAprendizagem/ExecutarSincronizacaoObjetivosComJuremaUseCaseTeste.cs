using FluentAssertions;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ObjetivoAprendizagem
{
    public class ExecutarSincronizacaoObjetivosComJuremaUseCaseTeste
    {
        private readonly Mock<IServicoObjetivosAprendizagem> _servicoMock;
        private readonly IExecutarSincronizacaoObjetivosComJuremaUseCase _useCase;

        public ExecutarSincronizacaoObjetivosComJuremaUseCaseTeste()
        {
            _servicoMock = new Mock<IServicoObjetivosAprendizagem>();
            _useCase = new ExecutarSincronizacaoObjetivosComJuremaUseCase(_servicoMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Chamado_Deve_Acionar_Servico_E_Retornar_True()
        {
            _servicoMock.Setup(s => s.SincronizarObjetivosComJurema()).Returns(Task.CompletedTask);

            var resultado = await _useCase.Executar(new MensagemRabbit());

            resultado.Should().BeTrue();
            _servicoMock.Verify(s => s.SincronizarObjetivosComJurema(), Times.Once);
        }
    }
}
