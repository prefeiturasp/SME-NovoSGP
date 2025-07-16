using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Aula.NotificacaoAlunosFaltosos
{
    public class NotificarAlunosFaltososUseCaseTeste
    {
        [Fact]
        public async Task Executar_DeveChamarPublicarFilaSgpParaCadaDreIdERetornarTrue()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();

            var dresIds = new List<long> { 1, 2, 3 };

            mediatorMock
                .Setup(m => m.Send(ObterIdsDresQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(dresIds);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var useCase = new NotificarAlunosFaltososUseCase(mediatorMock.Object);

            var mensagem = new MensagemRabbit("fake-payload");

            // Act
            var resultado = await useCase.Executar(mensagem);

            // Assert
            Assert.True(resultado);

            // Verifica se foi chamado uma vez para cada DRE
            foreach (var dreId in dresIds)
            {
                mediatorMock.Verify(m => m.Send(
                    It.Is<PublicarFilaSgpCommand>(cmd =>
                        cmd.Rota == RotasRabbitSgpAula.RotaNotificacaoAlunosFaltososDre &&
                        cmd.Filtros != null &&
                        cmd.Filtros.GetType() == typeof(DreDto) &&
                        ((DreDto)cmd.Filtros).DreCodigo == dreId
                    ),
                    It.IsAny<CancellationToken>()), Times.Once);
            }

            mediatorMock.Verify(m => m.Send(ObterIdsDresQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
