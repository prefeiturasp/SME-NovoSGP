using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.Grade.ObterGradePorTipoEscolaModalidadeDuracaoAno
{
    public class ObterGradePorTipoEscolaModalidadeDuracaoAnoQueryHandlerTeste
    {
        [Fact]
        public async Task Deve_Retornar_Grade_Valida_Quando_Encontrada()
        {
            // Arrange
            var gradeEsperada = new SME.SGP.Dominio.Grade
            {
                Id = 123,
                Nome = "Grade de Teste",
                CriadoEm = new DateTime(2024, 1, 1, 8, 0, 0),
                CriadoPor = "usuario.teste",
                CriadoRF = "1234567"
            };

            var repositorioMock = new Mock<IRepositorioGrade>();
            repositorioMock
                .Setup(repo => repo.ObterGradeTurmaAno(
                    TipoEscola.EMEF,
                    Modalidade.Fundamental,
                    9,
                    1,
                    "2025"))
                .ReturnsAsync(gradeEsperada);

            var handler = new ObterGradePorTipoEscolaModalidadeDuracaoAnoQueryHandler(repositorioMock.Object);

            var query = new ObterGradePorTipoEscolaModalidadeDuracaoAnoQuery(
                tipoEscola: TipoEscola.EMEF,
                modalidade: Modalidade.Fundamental,
                duracao: 9,
                ano: 1,
                anoLetivo: "2025"
            );

            // Act
            var resultado = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(123, resultado.Id);
            Assert.Equal("Grade de Teste", resultado.Nome);
            Assert.Equal("usuario.teste", resultado.CriadoPor);
            Assert.Equal("1234567", resultado.CriadoRF);
            Assert.Equal(new DateTime(2024, 1, 1, 8, 0, 0), resultado.CriadoEm);

            repositorioMock.Verify(repo => repo.ObterGradeTurmaAno(
                TipoEscola.EMEF, Modalidade.Fundamental, 9, 1, "2025"), Times.Once);
        }
    }
}
