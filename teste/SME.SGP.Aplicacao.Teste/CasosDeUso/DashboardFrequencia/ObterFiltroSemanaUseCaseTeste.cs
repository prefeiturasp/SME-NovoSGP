using Xunit;
using System;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DashboardFrequencia
{
    public class ObterFiltroSemanaUseCaseTeste
    {
        [Fact]
        public async Task Executar_DeveRetornarSemanasCorretas_ParaAnoLetivoPassado()
        {
            // Arrange
            var useCase = new ObterFiltroSemanaUseCase();
            int anoLetivo = DateTime.Now.Year - 1;

            // Act
            var resultado = await useCase.Executar(anoLetivo);

            // Assert
            Assert.NotNull(resultado);
            Assert.All(resultado, semana =>
            {
                Assert.True(semana.Inicio.DayOfWeek == DayOfWeek.Monday);
                Assert.True(semana.Fim >= semana.Inicio);
                Assert.True(semana.Fim <= new DateTime(anoLetivo, 12, 31));
            });
            Assert.True(resultado.Any());
        }

        [Fact]
        public async Task Executar_DeveRetornarSemanasCorretas_ParaAnoLetivoAtual()
        {
            // Arrange
            var useCase = new ObterFiltroSemanaUseCase();
            int anoLetivo = DateTime.Now.Year;

            // Act
            var resultado = await useCase.Executar(anoLetivo);

            // Assert
            Assert.NotNull(resultado);
            Assert.All(resultado, semana =>
            {
                Assert.True(semana.Inicio.DayOfWeek == DayOfWeek.Monday);
                Assert.True(semana.Fim >= semana.Inicio);
                Assert.True(semana.Fim <= DateTime.Now);
            });
            Assert.True(resultado.Any());
        }
    }
}
