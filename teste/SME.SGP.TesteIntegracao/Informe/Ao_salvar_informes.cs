using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
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
    public class Ao_salvar_informes : InformesBase
    {
        public Ao_salvar_informes(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<PublicarFilaSgpCommand, bool>), typeof(PublicarFilaSgpCommandFakeInforme), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterRfsUsuariosPorPerfisDreUeQuery, string[]>), typeof(ObterRfsUsuariosPorPerfisDreUeQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterGruposDeUsuariosQuery, IEnumerable<GruposDeUsuariosDto>>), typeof(ObterGruposDeUsuariosQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Informes - Salvar informes todas dre")]
        public async Task Ao_salvar_informes_todas_dre()
        {
            const string TITULO = "todas dres";
            await CriarDadosBase();
            var useCase = ServiceProvider.GetService<ISalvarInformesUseCase>();
            var dto = new InformesDto()
            {
                Perfis = new List<GruposDeUsuariosDto>()
                {
                    new GruposDeUsuariosDto()
                    {
                        Id = PERFIL_AD
                    },
                    new GruposDeUsuariosDto()
                    {
                        Id = PERFIL_ADM_COTIC
                    }
                },
                Titulo = TITULO,
                Texto = "teste"
            };
            var resultado = await useCase.Executar(dto);
            resultado.ShouldNotBeNull();

            var informes = ObterTodos<Informativo>().FirstOrDefault();
            informes.ShouldNotBeNull();
            informes.DreId.ShouldBeNull();
            informes.UeId.ShouldBeNull();
            informes.DataEnvio.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            informes.Titulo.ShouldBe(TITULO);
            informes.Texto.ShouldBe("teste");

            var informesPerfil = ObterTodos<InformativoPerfil>();
            informesPerfil.ShouldNotBeNull();
            informesPerfil.Count().ShouldBe(2);

            var informesNotificacao = ObterTodos<InformativoNotificacao>();
            informesNotificacao.ShouldNotBeNull();
            informesNotificacao.Count().ShouldBe(2);

            var notificacoes = ObterTodos<Notificacao>();
            notificacoes.ShouldNotBeNull();
            notificacoes.Count().ShouldBe(2);
        }

        [Fact(DisplayName = "Informes - Salvar informes por dre")]
        public async Task Ao_salvar_informes_por_dre()
        {
            const string TITULO = "por dre";
            await CriarDadosBase();
            var useCase = ServiceProvider.GetService<ISalvarInformesUseCase>();
            var dto = new InformesDto()
            {
                DreId = DRE_ID_1,
                Perfis = new List<GruposDeUsuariosDto>()
                {
                    new GruposDeUsuariosDto()
                    {
                        Id = PERFIL_AD
                    },
                    new GruposDeUsuariosDto()
                    {
                        Id = PERFIL_ADM_COTIC
                    }
                },
                Titulo = TITULO,
                Texto = "teste"
            };
            var resultado = await useCase.Executar(dto);
            resultado.ShouldNotBeNull();

            var informes = ObterTodos<Informativo>().FirstOrDefault();
            informes.ShouldNotBeNull();
            informes.DreId.ShouldBe(DRE_ID_1);
            informes.UeId.ShouldBeNull();
            informes.DataEnvio.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            informes.Titulo.ShouldBe(TITULO);
            informes.Texto.ShouldBe("teste");

            var informesPerfil = ObterTodos<InformativoPerfil>();
            informesPerfil.ShouldNotBeNull();
            informesPerfil.Count().ShouldBe(2);

            var informesNotificacao = ObterTodos<InformativoNotificacao>();
            informesNotificacao.ShouldNotBeNull();
            informesNotificacao.Count().ShouldBe(2);

            var notificacoes = ObterTodos<Notificacao>();
            notificacoes.ShouldNotBeNull();
            notificacoes.Count().ShouldBe(2);
        }

        [Fact(DisplayName = "Informes - Salvar informes por ue")]
        public async Task Ao_salvar_informes_por_ue()
        {
            const string TITULO = "por ue";
            await CriarDadosBase();
            var useCase = ServiceProvider.GetService<ISalvarInformesUseCase>();
            var dto = new InformesDto()
            {
                DreId = DRE_ID_1,
                UeId = UE_ID_1,
                Perfis = new List<GruposDeUsuariosDto>()
                {
                    new GruposDeUsuariosDto()
                    {
                        Id = PERFIL_AD
                    },
                    new GruposDeUsuariosDto()
                    {
                        Id = PERFIL_ADM_COTIC
                    }
                },
                Titulo = TITULO,
                Texto = "teste"
            };
            var resultado = await useCase.Executar(dto);
            resultado.ShouldNotBeNull();

            var informes = ObterTodos<Informativo>().FirstOrDefault();
            informes.ShouldNotBeNull();
            informes.DreId.ShouldBe(DRE_ID_1);
            informes.UeId.ShouldBe(UE_ID_1);
            informes.DataEnvio.ShouldBe(DateTimeExtension.HorarioBrasilia().Date);
            informes.Titulo.ShouldBe(TITULO);
            informes.Texto.ShouldBe("teste");

            var informesPerfil = ObterTodos<InformativoPerfil>();
            informesPerfil.ShouldNotBeNull();
            informesPerfil.Count().ShouldBe(2);

            var informesNotificacao = ObterTodos<InformativoNotificacao>();
            informesNotificacao.ShouldNotBeNull();
            informesNotificacao.Count().ShouldBe(2);

            var notificacoes = ObterTodos<Notificacao>();
            notificacoes.ShouldNotBeNull();
            notificacoes.Count().ShouldBe(2);
        }
    }
}
