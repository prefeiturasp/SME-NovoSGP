using Xunit;

namespace SME.SGP.Dominio
{
    public class ResponsavelExtensionTeste
    {
        [Theory]
        [InlineData("1", "Filiação 1")]
        [InlineData("2", "Filiação 2")]
        [InlineData("3", "Responsável Legal")]
        [InlineData("4", "Próprio estudante")]
        [InlineData(null, "Filiacao1")]
        [InlineData("", "Filiacao1")]
        [InlineData("99", "Filiacao1")]
        [InlineData("5", "Responsável Estrangeiro ou Naturalizado")]
        public void ObterTipoResponsavel_DeveRetornarNomeCorreto(string input, string expectedStart)
        {
            var result = Dominio.ResponsavelExtension.ObterTipoResponsavel(input);

            Assert.StartsWith(expectedStart, result);
        }
    }
}


