using Moq;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAbandono;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.PainelEducacional
{
    public class ObterAbandonoVisaoSmeDreQueryHandlerTeste
    {
        [Fact]
        public async Task Handle_DeveChamarRepositorioERetornarResultado()
        {
            var repositorioMock = new Mock<IRepositorioPainelEducacionalAbandono>();
            var resultadoEsperado = new List<PainelEducacionalAbandono> { new PainelEducacionalAbandono { Id = 1 } };
            repositorioMock
                .Setup(r => r.ObterAbandonoVisaoSmeDre(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(resultadoEsperado);
            var handler = new ObterAbandonoVisaoSmeDreQueryHandler(repositorioMock.Object);
            var query = new ObterAbandonoVisaoSmeDreQuery(2024, "dre", "ue");

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.Equal(resultadoEsperado, resultado);
            repositorioMock.Verify(r => r.ObterAbandonoVisaoSmeDre(2024, "dre", "ue"), Times.Once);
        }
    }
}
