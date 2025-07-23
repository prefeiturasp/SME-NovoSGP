using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ParecerConclusivo
{
    public class ReprocessarParecerConclusivoPorAnoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ReprocessarParecerConclusivoPorAnoUseCase _useCase;

        public ReprocessarParecerConclusivoPorAnoUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ReprocessarParecerConclusivoPorAnoUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DevePublicarComandoParaCadaDre()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var useCase = new ReprocessarParecerConclusivoPorAnoUseCase(mediatorMock.Object);
            var anoLetivo = 2025;

            var dres = new List<long> { 1, 2, 3 };
            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterIdsDresQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dres);

            var comandosRecebidos = new List<PublicarFilaSgpCommand>();

            mediatorMock
                .Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<bool>, CancellationToken>((cmd, _) =>
                {
                    var publicarCmd = (PublicarFilaSgpCommand)cmd;
                    comandosRecebidos.Add(publicarCmd);
                })
                .ReturnsAsync(true);

            // Act
            await useCase.Executar(anoLetivo);

            // Assert
            Assert.Equal(3, comandosRecebidos.Count);
            foreach (var dreId in dres)
            {
                var cmd = comandosRecebidos.FirstOrDefault(c =>
                    c.Rota == RotasRabbitSgpFechamento.RotaAtualizarParecerConclusivoAlunoPorDre &&
                    c.Filtros is FiltroDreUeTurmaDto filtro &&
                    filtro.AnoLetivo == anoLetivo &&
                    filtro.DreId == dreId
                );
                Assert.NotNull(cmd);
            }
            mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(3));
        }
    }
}
