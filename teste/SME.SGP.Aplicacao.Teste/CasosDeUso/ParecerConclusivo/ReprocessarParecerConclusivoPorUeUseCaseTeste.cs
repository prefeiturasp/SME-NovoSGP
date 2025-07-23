using MediatR;
using Moq;
using Newtonsoft.Json;
using Org.BouncyCastle.Utilities;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ParecerConclusivo
{
    public class ReprocessarParecerConclusivoPorUeUseCaseTeste
    {
        [Fact]
        public async Task Executar_DeveEnviarComandoParaCadaTurmaERetornarTrue()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();

            // Simular as turmas retornadas pela query
            var turmasSimuladas = new List<string> { "TURMA1", "TURMA2" };

            // Mock da query ObterCodigosTurmasPorUeAnoQuery para retornar as turmas simuladas
            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterCodigosTurmasPorUeAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turmasSimuladas);

            // Mock para aceitar qualquer PublicarFilaSgpCommand e retornar true
            mediatorMock
                .Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var useCase = new ReprocessarParecerConclusivoPorUeUseCase(mediatorMock.Object);

            var filtro = new FiltroDreUeTurmaDto(2025, 1, "UE123");
            var mensagem = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(filtro)
            };

            // Act
            var resultado = await useCase.Executar(mensagem);

            // Assert
            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Moq.Times.Exactly(turmasSimuladas.Count));
        }
    }
}
