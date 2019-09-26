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

            Usuario.Senha = "1aA";

            Usuario.validarSenha();

            Usuario.Senha = "Aa@";

            Usuario.validarSenha();
            
            Usuario.Senha = " 1a@";

            Assert.Throws<NegocioException>(Usuario.validarSenha);

            Usuario.Senha = @"1a@82193490!@#$%&*()";

            Assert.Throws<NegocioException>(Usuario.validarSenha);

            Usuario.Senha = "7710";

            Assert.Throws<NegocioException>(Usuario.validarSenha);

            Usuario.Senha = "Sgp7710";

            Assert.Throws<NegocioException>(Usuario.validarSenha);

            Assert.Throws<NegocioException>(() => { Usuario.validarSenha(new List<string> { "Sgp7710" }); });

            Usuario.Senha = "Sg7710";

            Usuario.validarSenha(new List<string> { "7710" });
        }
    }
}
