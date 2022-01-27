using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste
{
    public class ObterParametrosSistemaPorTiposQueryHandlerTeste
    {
        private readonly ObterParametrosSistemaPorTiposQueryHandler query;
        private readonly Mock<IRepositorioParametrosSistemaConsulta> repositorioParametrosSistema;


        public ObterParametrosSistemaPorTiposQueryHandlerTeste()
        {
            repositorioParametrosSistema = new Mock<IRepositorioParametrosSistemaConsulta>();
            query = new ObterParametrosSistemaPorTiposQueryHandler(repositorioParametrosSistema.Object);
        }
        [Fact]
        public async Task Deve_Consultar_Parametros_Por_Tipos()
        {
            //Arrange
            var listaRetorno = new List<ParametrosSistema>();
            repositorioParametrosSistema.Setup(a => a.ObterPorTiposAsync(It.IsAny<long[]>())).ReturnsAsync(listaRetorno);

            //Act
            var parametrosRetorno = await query.Handle(new ObterParametrosSistemaPorTiposQuery() { Tipos = new long[] { 1, 2 } }, new System.Threading.CancellationToken());

            //Assert
            repositorioParametrosSistema.Verify(c => c.ObterPorTiposAsync(It.IsAny<long[]>()), Times.Once);



        }

    }
}
