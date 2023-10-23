using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Informe.Base;
using SME.SGP.TesteIntegracao.Setup;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Informe
{
    public class Ao_excluir_informes : InformesBase
    {
        public Ao_excluir_informes(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Informes - Excluir informes todas dre")]
        public async Task Ao_excluir_informes_todas_dre()
        {
            await CriarDadosBase();

            await InserirNaBase(new Informativo()
            {
                Titulo = "informes todas dre",
                Texto = "teste",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new InformativoPerfil()
            {
                InformativoId = INFORME_ID_1,
                CodigoPerfil = PERFIL_AD,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new InformativoPerfil()
            {
                InformativoId = INFORME_ID_1,
                CodigoPerfil = PERFIL_ADM_COTIC,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ServiceProvider.GetService<IExcluirInformesUseCase>();

            await useCase.Executar(INFORME_ID_1);

            var informes = ObterTodos<Informativo>();
            informes.ShouldNotBeNull();
            informes.Any().ShouldBeFalse();

            var informesPerfil = ObterTodos<InformativoPerfil>();
            informesPerfil.ShouldNotBeNull();
            informesPerfil.Any().ShouldBeFalse();
        }


        [Fact(DisplayName = "Informes - Excluir informes por dre")]
        public async Task Ao_excluir_informes_por_dre()
        {
            await CriarDadosBase();

            await InserirNaBase(new Informativo()
            {
                DreId = DRE_ID_1,
                Titulo = "informes por dre",
                Texto = "teste",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new InformativoPerfil()
            {
                InformativoId = INFORME_ID_1,
                CodigoPerfil = PERFIL_AD,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new InformativoPerfil()
            {
                InformativoId = INFORME_ID_1,
                CodigoPerfil = PERFIL_ADM_COTIC,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var useCase = ServiceProvider.GetService<IExcluirInformesUseCase>();

            await useCase.Executar(INFORME_ID_1);

            var informes = ObterTodos<Informativo>();
            informes.ShouldNotBeNull();
            informes.Any().ShouldBeFalse();

            var informesPerfil = ObterTodos<InformativoPerfil>();
            informesPerfil.ShouldNotBeNull();
            informesPerfil.Any().ShouldBeFalse();
        }
    }
}
