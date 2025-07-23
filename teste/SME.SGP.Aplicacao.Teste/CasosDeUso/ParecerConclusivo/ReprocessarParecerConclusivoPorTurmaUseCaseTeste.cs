using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ParecerConclusivo
{
    public class ReprocessarParecerConclusivoPorTurmaUseCaseTeste
    {
        [Fact]        
        public async Task Executar_DeveEnviarComandoParaCadaRegistroERetornarTrue()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();

            var registrosSimulados = new List<ConselhoClasseFechamentoAlunoDto>
    {
        new ConselhoClasseFechamentoAlunoDto { },
        new ConselhoClasseFechamentoAlunoDto { }
    };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterConselhoClasseAlunosPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(registrosSimulados);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var useCase = new ReprocessarParecerConclusivoPorTurmaUseCase(mediatorMock.Object);

            var filtro = new FiltroDreUeTurmaDto(2025, 1, null);
            var mensagem = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(filtro)
            };

            // Act
            var resultado = await useCase.Executar(mensagem);

            // Assert
            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(registrosSimulados.Count));
        }
    }
}
