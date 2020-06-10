using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Comandos
{
    public class ComandosRelatorioCorrelacaoTeste
    {
        private readonly ComandoRelatorioCorrelacaoJasper comandoRelatorioCorrelacaoJasper;
        private readonly Mock<IRepositorioRelatorioCorrelacaoJasper> repositorioRelatorioCorrelacaoJasper;

        public ComandosRelatorioCorrelacaoTeste()
        {
            repositorioRelatorioCorrelacaoJasper = new Mock<IRepositorioRelatorioCorrelacaoJasper>();
            comandoRelatorioCorrelacaoJasper = new ComandoRelatorioCorrelacaoJasper(repositorioRelatorioCorrelacaoJasper.Object);
        }
        
        [Fact(DisplayName = "Deve_Salvar_Relatorio_Correlacao_Jasper")]
        public async Task Deve_Salvar_Relatorio_Correlacao_Jasper()
        {
            // Arrange
            var relatorioCorrelacao = new RelatorioCorrelacao(Dominio.Enumerados.TipoRelatorioEnum.RelatorioExemplo, new Usuario());
            var relatorioCorrelacaoJasper = new RelatorioCorrelacaoJasper(relatorioCorrelacao, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            // Act
            var result = await comandoRelatorioCorrelacaoJasper.Salvar(relatorioCorrelacaoJasper);

            // Assert
            Assert.Equal(1, result);

        }
    }
}
