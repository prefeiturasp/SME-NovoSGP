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
        //private readonly ComandoRelatorioCorrelacaoJasper comandoRelatorioCorrelacaoJasper;
        //private readonly Mock<IRepositorioCorrelacaoRelatorioJasper> repositorioRelatorioCorrelacaoJasper;

        //public ComandosRelatorioCorrelacaoTeste()
        //{
        //    repositorioRelatorioCorrelacaoJasper = new Mock<IRepositorioCorrelacaoRelatorioJasper>();
        //    comandoRelatorioCorrelacaoJasper = new ComandoRelatorioCorrelacaoJasper(repositorioRelatorioCorrelacaoJasper.Object);
        //}
        
        //[Fact(DisplayName = "Deve_Salvar_Relatorio_Correlacao_Jasper")]
        //public async Task Deve_Salvar_Relatorio_Correlacao_Jasper()
        //{
        //    // Arrange
        //    var relatorioCorrelacao = new RelatorioCorrelacao(Dominio.Enumerados.TipoRelatorio.RelatorioExemplo, new Usuario());
        //    var relatorioCorrelacaoJasper = new RelatorioCorrelacaoJasper(relatorioCorrelacao, "123", Guid.NewGuid(), Guid.NewGuid());
        //    repositorioRelatorioCorrelacaoJasper.Setup(c => c.Salvar(It.IsAny<RelatorioCorrelacaoJasper>()))
        //        .Returns(1);

        //    // Act
        //    var result = await comandoRelatorioCorrelacaoJasper.Salvar(relatorioCorrelacaoJasper);

        //    // Assert
        //    Assert.Equal(1, result);

        //}
    }
}
