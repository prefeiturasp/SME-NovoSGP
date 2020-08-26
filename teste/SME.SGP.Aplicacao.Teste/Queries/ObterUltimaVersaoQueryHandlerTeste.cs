using Moq;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Queries.Github.ObterVersaoRelease;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries
{
    public class ObterUltimaVersaoQueryHandlerTeste
    {
        private readonly ObterUltimaVersaoQueryHandler obterUltimaVersaoQueryHandler;
        private readonly Mock<IServicoGithub> servicoGithub;
        private readonly Mock<IRepositorioCache> repositorioCache;

        public ObterUltimaVersaoQueryHandlerTeste()
        {
            repositorioCache = new Mock<IRepositorioCache>();
            servicoGithub = new Mock<IServicoGithub>();
            obterUltimaVersaoQueryHandler = new ObterUltimaVersaoQueryHandler(servicoGithub.Object, repositorioCache.Object);
        }
        [Fact]
        public async Task Deve_Obter_Versao_Com_Cache()
        {
            //Arrange
            servicoGithub.Setup(a => a.RecuperarUltimaVersao())
                .ReturnsAsync("v1");

            repositorioCache.Setup(a => a.ObterAsync("versao", false))
                .ReturnsAsync("v1");

            //Act
            var versao = await obterUltimaVersaoQueryHandler.Handle(new ObterUltimaVersaoQuery(), new CancellationToken());

            //Asert
            repositorioCache.Verify(x => x.ObterAsync("versao", false), Times.Once);
            servicoGithub.Verify(x => x.RecuperarUltimaVersao(), Times.Never);

            Assert.True(versao == "v1");


        }
        [Fact]
        public async Task Deve_Obter_Versao_Sem_Cache()
        {
            //Arrange
            servicoGithub.Setup(a => a.RecuperarUltimaVersao())
                .ReturnsAsync("v1");

            repositorioCache.Setup(a => a.ObterAsync("versao", false))
                .ReturnsAsync(string.Empty);

            //Act
            var versao = await obterUltimaVersaoQueryHandler.Handle(new ObterUltimaVersaoQuery(), new CancellationToken());

            //Asert
            repositorioCache.Verify(x => x.ObterAsync("versao", false), Times.Once);
            servicoGithub.Verify(x => x.RecuperarUltimaVersao(), Times.Once);

            Assert.True(versao == "v1");


        }

    }
}
