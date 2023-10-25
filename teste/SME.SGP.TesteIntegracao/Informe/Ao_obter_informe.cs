using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.Informe.Base;
using SME.SGP.TesteIntegracao.Informe.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Informe
{
    public class Ao_obter_informe : InformesBase
    {
        public Ao_obter_informe(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterGruposDeUsuariosQuery, IEnumerable<GruposDeUsuariosDto>>), typeof(ObterGruposDeUsuariosQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Informes - Obter informe sem dre")]
        public async Task Ao_obter_informe_sem_dre()
        {
            await CriarDadosBase();

            await InserirNaBase(new Informativo()
            {
                Titulo = "informes sem dre",
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

            var useCase = ServiceProvider.GetService<IObterInformeUseCase>();

            var resultado = await useCase.Executar(INFORME_ID_1);
            resultado.ShouldNotBeNull();
            resultado.DreNome.ShouldBe("Todas");
            resultado.UeNome.ShouldBe("Todas");
            resultado.Texto.ShouldBe("teste");
            resultado.Titulo.ShouldBe("informes sem dre");
            resultado.Perfis.ShouldNotBeNull();
            resultado.Perfis.ToList().Exists(p => p.Id == PERFIL_AD).ShouldBeTrue();
            resultado.Perfis.ToList().Exists(p => p.Id == PERFIL_ADM_COTIC).ShouldBeTrue();
            resultado.Auditoria.ShouldNotBeNull();
            resultado.Auditoria.CriadoPor.ShouldBe(SISTEMA_NOME);
        }


        [Fact(DisplayName = "Informes - Obter informe com ue")]
        public async Task Ao_obter_informe_com_ue()
        {
            await CriarDadosBase();

            await InserirNaBase(new Informativo()
            {
                DreId = DRE_ID_1,
                UeId = UE_ID_1,
                Titulo = "informes com ue",
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

            var useCase = ServiceProvider.GetService<IObterInformeUseCase>();

            var resultado = await useCase.Executar(INFORME_ID_1);
            resultado.ShouldNotBeNull();
            resultado.DreNome.ShouldBe(DRE_NOME_1);
            resultado.UeNome.ShouldBe($"{TipoEscola.EMEF.ShortName()} {UE_NOME_1}");
            resultado.Texto.ShouldBe("teste");
            resultado.Titulo.ShouldBe("informes com ue");
            resultado.Perfis.ShouldNotBeNull();
            resultado.Perfis.ToList().Exists(p => p.Id == PERFIL_AD).ShouldBeTrue();
            resultado.Perfis.ToList().Exists(p => p.Id == PERFIL_ADM_COTIC).ShouldBeTrue();
            resultado.Auditoria.ShouldNotBeNull();
            resultado.Auditoria.CriadoPor.ShouldBe(SISTEMA_NOME);
        }
    }
}
