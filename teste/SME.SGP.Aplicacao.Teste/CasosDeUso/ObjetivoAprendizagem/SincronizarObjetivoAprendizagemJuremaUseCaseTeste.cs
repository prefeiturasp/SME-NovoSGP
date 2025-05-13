using System.Threading.Tasks;
using Moq;
using Xunit;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Aplicacao.CasosDeUso;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class SincronizarObjetivoAprendizagemJuremaUseCaseTeste
    {
        private readonly Mock<IServicoObjetivosAprendizagem> servicoObjetivosAprendizagemMock;
        private readonly ExecutarSincronizacaoObjetivosComJuremaUseCase useCase;

        public SincronizarObjetivoAprendizagemJuremaUseCaseTeste()
        {
            servicoObjetivosAprendizagemMock = new Mock<IServicoObjetivosAprendizagem>();
            useCase = new ExecutarSincronizacaoObjetivosComJuremaUseCase(servicoObjetivosAprendizagemMock.Object);
        }

        [Fact]
        public async Task Deve_Sincronizar_Objetivos_Com_Jurema()
        {
            // Arrange
            servicoObjetivosAprendizagemMock
                .Setup(s => s.SincronizarObjetivosComJurema())
                .Returns(Task.CompletedTask);

            var mensagemRabbitFake = new MensagemRabbit(); 

            // Act
            var resultado = await useCase.Executar(mensagemRabbitFake);

            // Assert
            servicoObjetivosAprendizagemMock.Verify(s => s.SincronizarObjetivosComJurema(), Times.Once);
            Assert.True(resultado); // porque o método Executar retorna bool true
        }
    }

}
