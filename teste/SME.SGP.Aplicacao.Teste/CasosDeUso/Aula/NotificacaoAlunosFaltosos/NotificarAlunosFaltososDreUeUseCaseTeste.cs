using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Aula.NotificacaoAlunosFaltosos
{
    public class NotificarAlunosFaltososDreUeUseCaseTeste
    {
        [Fact]
        public async Task Executar_DeveNotificarQuandoFiltroValido()
        {
            // Arrange
            var servicoMock = new Mock<IServicoNotificacaoFrequencia>();
            var useCase = new NotificarAlunosFaltososDreUeUseCase(servicoMock.Object);

            var filtro = new DreUeDto(dreId: 1, ueId: 99, codigoUe: "UE123");

            // Aqui serializa o objeto para JSON
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            // Act
            var resultado = await useCase.Executar(mensagem);

            // Assert
            Assert.True(resultado);
            servicoMock.Verify(s => s.NotificarAlunosFaltosos(filtro.UeId), Times.Once);
        }

        [Fact]
        public async Task Executar_DeveRetornarFalseQuandoFiltroNulo()
        {
            // Arrange
            var servicoMock = new Mock<IServicoNotificacaoFrequencia>();
            var useCase = new NotificarAlunosFaltososDreUeUseCase(servicoMock.Object);

            var mensagem = new MensagemRabbit("null");

            // Act
            var resultado = await useCase.Executar(mensagem);

            // Assert
            Assert.False(resultado);
            servicoMock.Verify(s => s.NotificarAlunosFaltosos(It.IsAny<long>()), Times.Never);
        }
    }
}
