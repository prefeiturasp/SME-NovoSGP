using MediatR;
using Moq;
using System;
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
            var mockRetorno = 1000;

            mediator.Setup(a => a.Send(It.IsAny<ObterPlanejamentoAnualPorTurmaComponenteQuery>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(mockRetorno);

            var retorno = await useCase.Executar(1,1);

            mediator.Verify(x => x.Send(It.IsAny<ObterPlanejamentoAnualPorTurmaComponenteQuery>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(retorno == 1000);
        }

        [Fact]
        public void Deve_Lancar_ArgumentNullException_Quando_Mediator_For_Nulo()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => 
                new ObterPlanejamentoAnualPorTurmaComponenteUseCase(null));
            
            Assert.Equal("mediator", exception.ParamName);
        }
    }
}
