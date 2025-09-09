using Xunit;

namespace SME.SGP.Dominio.Teste
{
    public class DreTeste
    {
        [Fact]
        public void PrefixoDoNomeAbreviado_DeveRetornarNomeAbreviadoCorretamente()
        {
            // Arrange
            var dre = new Dre
            {
                Nome = "DIRETORIA REGIONAL DE EDUCACAO - SUL"
            };
            // Act
            var resultado = dre.PrefixoDoNomeAbreviado;
            // Assert
            Assert.Equal("DRE - SUL", resultado);
        }
    }
}
