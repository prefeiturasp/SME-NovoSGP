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
        }

        [Fact]
        public void DevePermitirCriarEvento()
        {
            var perfisUsuario = new List<PrioridadePerfil>
            {
                new PrioridadePerfil
                {
                    Tipo=TipoPerfil.SME
                }
            };
            var evento = new Evento
            {
                TipoEvento = new Entidades.EventoTipo
                {
                    LocalOcorrencia = EventoLocalOcorrencia.SME
                }
            };
            var usuario = new Usuario();
            usuario.DefinirPerfis(perfisUsuario);
            usuario.PodeCriarEvento(evento);
            Assert.True(true);
        }

        [Fact]
        public void DevePermitirCriarEventoSemPerfilDeSMEouDRE()
        {
            var perfisUsuario = new List<PrioridadePerfil>
            {
                new PrioridadePerfil
                {
                    Tipo=TipoPerfil.UE
                }
            };
            var evento = new Evento
            {
                TipoEvento = new Entidades.EventoTipo
                {
                    LocalOcorrencia = EventoLocalOcorrencia.UE
                },
                DreId = "123",
                UeId = "123"
            };
            var usuario = new Usuario();
            usuario.DefinirPerfis(perfisUsuario);
            usuario.PodeCriarEvento(evento);
            Assert.True(true);
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

        [Fact]
        public void NaoDevePermitirCriarEventoSemInformarDREOuUE()
        {
            var perfisUsuario = new List<PrioridadePerfil>
            {
                new PrioridadePerfil
                {
                    Tipo=TipoPerfil.UE
                }
            };
            var evento = new Evento
            {
                TipoEvento = new Entidades.EventoTipo
                {
                    LocalOcorrencia = EventoLocalOcorrencia.SME
                },
                UeId = "123"
            };
            var usuario = new Usuario();
            usuario.DefinirPerfis(perfisUsuario);
            var erro = Assert.Throws<NegocioException>(() => usuario.PodeCriarEvento(evento));
            Assert.Equal("É necessário informar a DRE.", erro.Message);

            evento.DreId = "123";
            evento.UeId = "";
            erro = Assert.Throws<NegocioException>(() => usuario.PodeCriarEvento(evento));
            Assert.Equal("É necessário informar a UE.", erro.Message);
        }

        [Fact]
        public void NaoDevePermitirCriarEventoSemPerfilDeSME()
        {
            var perfisUsuario = new List<PrioridadePerfil>
            {
                new PrioridadePerfil
                {
                    Tipo=TipoPerfil.UE
                }
            };
            var evento = new Evento
            {
                TipoEvento = new Entidades.EventoTipo
                {
                    LocalOcorrencia = EventoLocalOcorrencia.SME
                },
                DreId = "123",
                UeId = "123"
            };
            var usuario = new Usuario();
            usuario.DefinirPerfis(perfisUsuario);
            var erro = Assert.Throws<NegocioException>(() => usuario.PodeCriarEvento(evento));
            Assert.Equal("Somente usuários da SME podem criar este tipo de evento.", erro.Message);
        }
    }
}