using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Aula.Pendencia
{
    public class PendenciaAulaDreUseCaseTeste
    {
        [Fact]
        public async Task Executar_DevePublicarPendenciasQuandoFiltroValido()
        {
            // Arrange
            var dreId = 123;
            var dreDto = new DreDto(dreId);
            var mensagem = new MensagemRabbit(Newtonsoft.Json.JsonConvert.SerializeObject(dreDto));

            var ues = new List<Ue>
        {
            new Ue { Id = 1, CodigoUe = "UE1" },
            new Ue { Id = 2, CodigoUe = "UE2" }
        };

            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(m => m.Send(It.Is<ObterUesPorDreCodigoQuery>(q => q.DreId == dreId), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(ues);

            mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            var useCase = new PendenciaAulaDreUseCase(mediatorMock.Object);

            // Act
            var resultado = await useCase.Executar(mensagem);

            // Assert
            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Executar_DeveRetornarFalseQuandoFiltroNulo()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();

            var useCase = new PendenciaAulaDreUseCase(mediatorMock.Object);

            // Simula um JSON "null" para o deserializador não lançar exceção
            var mensagem = new MensagemRabbit("null");

            // Act
            var resultado = await useCase.Executar(mensagem);

            // Assert
            Assert.False(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
