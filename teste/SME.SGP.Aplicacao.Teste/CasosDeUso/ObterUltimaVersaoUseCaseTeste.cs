using MediatR;
using Moq;
using SME.SGP.Aplicacao.Queries.Github.ObterVersaoRelease;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class ObterUltimaVersaoUseCaseTeste
    {
        private readonly ObterUltimaVersaoUseCase obterUltimaVersaoUseCase;
        private readonly Mock<IMediator> mediator;

        public ObterUltimaVersaoUseCaseTeste()
        {

            mediator = new Mock<IMediator>();
            obterUltimaVersaoUseCase = new ObterUltimaVersaoUseCase(mediator.Object);
        }
        [Fact]
        public async Task Deve_Obter_Versao()
        {
            //Arrange
            mediator.Setup(a => a.Send(It.IsAny<ObterUltimaVersaoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("v1");

            //Act
            var versao = await obterUltimaVersaoUseCase.Executar();

            //Asert
            mediator.Verify(x => x.Send(It.IsAny<ObterUltimaVersaoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.True(versao == "v1");

            
        }
    }
}

