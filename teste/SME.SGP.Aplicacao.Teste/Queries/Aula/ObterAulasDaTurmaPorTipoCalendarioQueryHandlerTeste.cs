using Moq;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.Aula
{
    public class ObterAulasDaTurmaPorTipoCalendarioQueryHandlerTeste
    {
        [Fact]
        public void Construtor_Deve_Lancar_ArgumentNullException_QuandoRepositorioEhNulo()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterAulasDaTurmaPorTipoCalendarioQueryHandler(null));
        }

        [Fact]
        public async Task Handle_Deve_Chamar_RepositorioE_RetornarAulas()
        {
            var mockRepositorio = new Mock<IRepositorioAulaConsulta>();
            var aulasEsperadas = new List<SME.SGP.Dominio.Aula> { new SME.SGP.Dominio.Aula(), new SME.SGP.Dominio.Aula() };
            mockRepositorio.Setup(r => r.ObterAulasPorTurmaETipoCalendario(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(aulasEsperadas);
            var handler = new ObterAulasDaTurmaPorTipoCalendarioQueryHandler(mockRepositorio.Object);
            var query = new ObterAulasDaTurmaPorTipoCalendarioQuery("TURMA1", 123, "usuario");

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.Equal(aulasEsperadas, resultado);
            mockRepositorio.Verify(r => r.ObterAulasPorTurmaETipoCalendario(123, "TURMA1", "usuario"), Times.Once);
        }
    }
}
