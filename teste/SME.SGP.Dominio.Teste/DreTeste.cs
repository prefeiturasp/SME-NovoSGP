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

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void PrefixoDoNomeAbreviado_DeveRetornarMesmoNome_QuandoNomeForNuloOuVazio(string nomeInvalido)
        {
            // Arrange
            var dre = new Dre
            {
                Nome = nomeInvalido
            };
            // Act
            var resultado = dre.PrefixoDoNomeAbreviado;
            // Assert
            Assert.Equal(nomeInvalido, resultado);
        }
    }
}
