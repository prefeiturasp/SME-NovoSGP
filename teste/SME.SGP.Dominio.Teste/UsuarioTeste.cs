using Xunit;

namespace SME.SGP.Dominio.Teste
{
    public class UsuarioTeste
    {
        [Fact]
        public void DeveValidarSenha()
        {
            var Usuario = new Usuario();

            Usuario.CodigoRf = "7777710";

            Usuario.ValidarSenha("1aA23233");

            Usuario.ValidarSenha("Aa@dfgsdfg");

            Assert.Throws<NegocioException>(() => Usuario.ValidarSenha("1a@egrgeg"));

            Assert.Throws<NegocioException>(() => Usuario.ValidarSenha(@"1aA@82193490!@#$%&*()"));

            Assert.Throws<NegocioException>(() => Usuario.ValidarSenha("7710"));

            Assert.Throws<NegocioException>(() => Usuario.ValidarSenha("Sgp7710"));
        }
    }
}