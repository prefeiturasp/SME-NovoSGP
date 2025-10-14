using Moq;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAbandono;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.PainelEducacional
{
    public class ObterAbandonoUeQueryHandlerTeste
    {
        [Fact]
        public async Task DeveRetornarDtoCorretoAoConsultarRepositorio()
        {
            var mockRepo = new Mock<IRepositorioPainelEducacionalAbandonoUe>();
            var paginacaoResultado = new PaginacaoResultadoDto<PainelEducacionalAbandonoUe>
            {
                Items = new List<PainelEducacionalAbandonoUe>
                {
                    new PainelEducacionalAbandonoUe {
                        CodigoTurma = "T1",
                        NomeTurma = "Turma 1",
                        Modalidade = "CIEJA",
                        QuantidadeDesistencias = 5
                    }
                },
                TotalPaginas = 1,
                TotalRegistros = 1
            };
            mockRepo.Setup(r => r.ObterAbandonoUe(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(paginacaoResultado);

            var handler = new ObterAbandonoUeQueryHandler(mockRepo.Object);
            var query = new ObterAbandonoUeQuery(2025, "dre", "ue", "CIEJA", 1, 10);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Single(result.Modalidades);
            Assert.Equal("Turma 1", result.Modalidades[0].Turma);
            Assert.Equal(1, result.TotalPaginas);
            Assert.Equal(1, result.TotalRegistros);
        }
    }
}
