using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Autenticar
{
    public class Ao_realizar_suporte_com_perfil_adm_dre : TesteBaseComuns
    {
        public Ao_realizar_suporte_com_perfil_adm_dre(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Ao_realizar_suporte_com_perfil_adm_dre_para_usuario_perfil_naapa()
        {
            CriarClaimUsuario(Perfis.PERFIL_ADMDRE.ToString());

            await CriaUsuarios();
            var comando = ServiceProvider.GetService<IComandosUsuario>();
            var excecao = await Assert.ThrowsAsync<NegocioException>(() => comando.AutenticarSuporte(USUARIO_LOGIN_COOD_NAAPA));

            excecao.Message.ShouldBe(MensagemNegocioComuns.ACESSO_SUPORTE_INDISPONIVEL);
        }

        [Fact]
        public async Task Ao_realizar_suporte_com_perfil_adm_sme_para_usuario_perfil_naapa()
        {
            CriarClaimUsuario(Perfis.PERFIL_ADMSME.ToString());

            await CriaUsuarios();
            var comando = ServiceProvider.GetService<IComandosUsuario>();
            var retorno = await comando.AutenticarSuporte(USUARIO_LOGIN_COOD_NAAPA);

            retorno.ShouldNotBeNull();

            retorno.Autenticado.ShouldBeTrue();
            retorno.AdministradorSuporte.ShouldNotBeNull();
            retorno.AdministradorSuporte.Login.ShouldBe(USUARIO_LOGIN_ADM_SME);
        }

        private async Task CriaUsuarios()
        {
            await InserirNaBase(new Usuario()
            {
                CodigoRf = USUARIO_LOGIN_ADM_SME,
                Login = USUARIO_LOGIN_ADM_SME,
                Nome = USUARIO_LOGADO_NOME,
                PerfilAtual = Guid.Parse(PerfilUsuario.ADMSME.ObterNome()),
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
            });

            await InserirNaBase(new PrioridadePerfil()
            {
                Ordem = 239,
                Tipo = TipoPerfil.SME,
                NomePerfil = "Perfil SME",
                CodigoPerfil = Perfis.PERFIL_ADMSME,
                CriadoEm = DateTime.Now,
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new Usuario()
            {
                CodigoRf = USUARIO_LOGIN_ADM_DRE,
                Login = USUARIO_LOGIN_ADM_DRE,
                Nome = USUARIO_LOGADO_NOME,
                PerfilAtual = Guid.Parse(PerfilUsuario.ADMDRE.ObterNome()),
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
            });

            await InserirNaBase(new PrioridadePerfil()
            {
                Ordem = 240,
                Tipo = TipoPerfil.DRE,
                NomePerfil = "Perfil dre",
                CodigoPerfil = Perfis.PERFIL_ADMDRE,
                CriadoEm = DateTime.Now,
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new Usuario()
            {
                CodigoRf = USUARIO_LOGIN_COOD_NAAPA,
                Login = USUARIO_LOGIN_COOD_NAAPA,
                Nome = "Coordenador",
                PerfilAtual = Guid.Parse(PerfilUsuario.COORDENADOR_NAAPA.ObterNome()),
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 01),
            });

            await InserirNaBase(new PrioridadePerfil()
            {
                Ordem = 241,
                Tipo = TipoPerfil.UE,
                NomePerfil = "Coordenador",
                CodigoPerfil = Perfis.PERFIL_COORDENADOR_NAAPA,
                CriadoEm = DateTime.Now,
                CriadoPor = "",
                CriadoRF = ""
            });
        }
    }
}
