using Moq;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAbandono;
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
        public async Task Handle_DeveRetornarDtoAgrupadoPorModalidadeEAno()
        {
            var repositorioMock = new Mock<IRepositorioPainelEducacionalAbandono>();
            var registros = new List<SME.SGP.Dominio.Entidades.PainelEducacionalAbandono>
            {
                new SME.SGP.Dominio.Entidades.PainelEducacionalAbandono { Modalidade = "Fundamental", Ano = "2024", QuantidadeDesistencias = 2 },
                new SME.SGP.Dominio.Entidades.PainelEducacionalAbandono { Modalidade = "Fundamental", Ano = "2024", QuantidadeDesistencias = 3 },
                new SME.SGP.Dominio.Entidades.PainelEducacionalAbandono { Modalidade = "Médio", Ano = "2024", QuantidadeDesistencias = 1 }
            };
            repositorioMock.Setup(r => r.ObterAbandonoVisaoSmeDre(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(registros);
            var handler = new ObterAbandonoVisaoSmeDreQueryHandler(repositorioMock.Object);
            var query = new ObterAbandonoVisaoSmeDreQuery(2024, "dre");

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.Single(resultado);
            var dto = Assert.Single(resultado);
            Assert.Equal(2, dto.Modalidades.Count);
        }
    }
}
