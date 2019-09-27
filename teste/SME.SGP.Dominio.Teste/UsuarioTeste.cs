using System;
using System.Collections.Generic;
using System.Text;
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

            Usuario.Senha = "1aA23233";

            Usuario.validarSenha();

            Usuario.Senha = "Aa@dfgsdfg";

            Usuario.validarSenha();
            
            Usuario.Senha = "1a@egrgeg";

            Assert.Throws<NegocioException>(Usuario.validarSenha);

            Usuario.Senha = @"1aA@82193490!@#$%&*()";

            Assert.Throws<NegocioException>(Usuario.validarSenha);

            Usuario.Senha = "7710";

            Assert.Throws<NegocioException>(Usuario.validarSenha);

            Usuario.Senha = "Sgp7710";

            Assert.Throws<NegocioException>(Usuario.validarSenha);            
        }
    }
}
