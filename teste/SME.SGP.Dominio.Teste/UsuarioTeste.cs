using System.Collections.Generic;
using Xunit;

namespace SME.SGP.Dominio.Teste
{
    public class UsuarioTeste
    {
        [Fact]
        public void DeveDefinirNovoEmailUsuarioSME()
        {
            var perfisUsuario = new List<PrioridadePerfil>
            {
                new PrioridadePerfil
                {
                    Tipo=TipoPerfil.SME
                }
            };
            var usuario = new Usuario();
            var novoEmail = "teste@sme.prefeitura.sp.gov.br";
            usuario.DefinirPerfis(perfisUsuario);
            usuario.DefinirEmail(novoEmail);
            Assert.Equal(usuario.Email, novoEmail);
        }

        [Fact]
        public void DeveDefinirNovoEmailUsuarioUE()
        {
            var perfisUsuario = new List<PrioridadePerfil>
            {
                new PrioridadePerfil
                {
                    Tipo=TipoPerfil.UE
                }
            };
            var usuario = new Usuario();
            var novoEmail = "teste@gmail.com";
            usuario.DefinirPerfis(perfisUsuario);
            usuario.DefinirEmail(novoEmail);
            Assert.Equal(usuario.Email, novoEmail);
        }

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

        [Fact]
        public void NaoDeveDefinirNovoEmailUsuario()
        {
            var perfisUsuario = new List<PrioridadePerfil>
            {
                new PrioridadePerfil
                {
                    Tipo=TipoPerfil.SME
                }
            };
            var usuario = new Usuario();
            var novoEmail = "teste@gmail.com";
            usuario.DefinirPerfis(perfisUsuario);
            Assert.Throws<NegocioException>(() => usuario.DefinirEmail(novoEmail));
        }
    }
}