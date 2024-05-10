using MediatR;
using Moq;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class ObterPlanejamentoAnualPorTurmaComponenteUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterPlanejamentoAnualPorTurmaComponenteUseCase useCase;

        public ObterPlanejamentoAnualPorTurmaComponenteUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterPlanejamentoAnualPorTurmaComponenteUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Planejamento_Id()
        {
            //Arrange
            var mockRetorno = 1000;

            mediator.Setup(a => a.Send(It.IsAny<ObterPlanejamentoAnualPorTurmaComponenteQuery>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(mockRetorno);

//Act
            var retorno = await useCase.Executar(1,1);

            //Asert
            mediator.Verify(x => x.Send(It.IsAny<ObterPlanejamentoAnualPorTurmaComponenteQuery>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(retorno == 1000);
        }
    }
}
