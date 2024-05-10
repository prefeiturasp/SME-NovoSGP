using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Dtos.Informes;
using SME.SGP.TesteIntegracao.Informe.Base;
using SME.SGP.TesteIntegracao.Informe.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Informe
{
    public class Ao_obter_infomes_filtro : InformesBase
    {
        public Ao_obter_infomes_filtro(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterGruposDeUsuariosQuery, IEnumerable<GruposDeUsuariosDto>>), typeof(ObterGruposDeUsuariosQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Informes - Obter informes sem filtro")]
        public async Task Ao_obter_informes_sem_filtro()
        {
            await CriarDadosBase();

            await InserirNaBase(new Informativo()
            {
                Titulo = "informe 1",
                Texto = "teste 1",
                DataEnvio = DateTimeExtension.HorarioBrasilia(),
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

            await InserirNaBase(new Informativo()
            {
                DreId = DRE_ID_1,
                UeId = UE_ID_1,
                Titulo = "informe 2",
                Texto = "teste 2",
                DataEnvio = DateTimeExtension.HorarioBrasilia(),
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new InformativoPerfil()
            {
                InformativoId = INFORME_ID_2,
                CodigoPerfil = PERFIL_ADM_UE,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var filtro = new InformeFiltroDto();
            var useCase = ServiceProvider.GetService<IObterInformesPorFiltroUseCase>();
            var resultado = await useCase.Executar(filtro);

            resultado.ShouldNotBeNull();
            resultado.TotalPaginas.ShouldBe(1);
            resultado.TotalRegistros.ShouldBe(2);
            resultado.Items.ShouldNotBeNull();
            resultado.Items.Count().ShouldBe(2);
            var item1 = resultado.Items.FirstOrDefault(item => item.Id == 1);
            item1.DreNome.ShouldBe("Todas");
            item1.UeNome.ShouldBe("Todas");
            item1.Titulo.ShouldBe("informe 1");
            item1.DataEnvio.ShouldBe(DateTimeExtension.HorarioBrasilia().ToString("dd/MM/yyyy"));
            item1.EnviadoPor.ShouldBe($"{SISTEMA_NOME} ({SISTEMA_CODIGO_RF})");
            var item2 = resultado.Items.FirstOrDefault(item => item.Id == 2);
            item2.DreNome.ShouldBe(DRE_NOME_1);
            item2.UeNome.ShouldBe($"{TipoEscola.EMEF.ShortName()} {UE_NOME_1}");
            item2.Titulo.ShouldBe("informe 2");
            item2.DataEnvio.ShouldBe(DateTimeExtension.HorarioBrasilia().ToString("dd/MM/yyyy"));
            item2.EnviadoPor.ShouldBe($"{SISTEMA_NOME} ({SISTEMA_CODIGO_RF})");
        }

        [Fact(DisplayName = "Informes - Obter informes todos filtros")]
        public async Task Ao_obter_informes_todos_filtros()
        {
            await CriarDadosBase();

            await InserirNaBase(new Informativo()
            {
                Titulo = "informe 1",
                Texto = "teste 1",
                DataEnvio = DateTimeExtension.HorarioBrasilia(),
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

            await InserirNaBase(new Informativo()
            {
                DreId = DRE_ID_1,
                UeId = UE_ID_1,
                Titulo = "informe 2",
                Texto = "teste 2",
                DataEnvio = DateTimeExtension.HorarioBrasilia(),
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new InformativoPerfil()
            {
                InformativoId = INFORME_ID_2,
                CodigoPerfil = PERFIL_ADM_UE,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var filtro = new InformeFiltroDto()
            {
                DreId = DRE_ID_1,
                UeId = UE_ID_1,
                Titulo = "informe",
                Perfis = new List<long> { PERFIL_ADM_UE },
                DataEnvioInicio = DateTimeExtension.HorarioBrasilia(),
                DataEnvioFim = DateTimeExtension.HorarioBrasilia().AddMonths(1)
            };

            var useCase = ServiceProvider.GetService<IObterInformesPorFiltroUseCase>();
            var resultado = await useCase.Executar(filtro);

            resultado.ShouldNotBeNull();
            resultado.TotalPaginas.ShouldBe(1);
            resultado.TotalRegistros.ShouldBe(1);
            resultado.Items.ShouldNotBeNull();
            resultado.Items.Count().ShouldBe(1);
            var item2 = resultado.Items.FirstOrDefault(item => item.Id == 2);
            item2.DreNome.ShouldBe(DRE_NOME_1);
            item2.UeNome.ShouldBe($"{TipoEscola.EMEF.ShortName()} {UE_NOME_1}");
            item2.Titulo.ShouldBe("informe 2");
            item2.DataEnvio.ShouldBe(DateTimeExtension.HorarioBrasilia().ToString("dd/MM/yyyy"));
            item2.EnviadoPor.ShouldBe($"{SISTEMA_NOME} ({SISTEMA_CODIGO_RF})");
        }

        [Fact(DisplayName = "Informes - Obter informes filtros dre e ue")]
        public async Task Ao_obter_informes_filtros_dre_ue()
        {
            await CriarDadosBase();

            await InserirNaBase(new Informativo()
            {
                Titulo = "informe 1",
                Texto = "teste 1",
                DataEnvio = DateTimeExtension.HorarioBrasilia(),
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

            await InserirNaBase(new Informativo()
            {
                DreId = DRE_ID_1,
                UeId = UE_ID_1,
                Titulo = "informe 2",
                Texto = "teste 2",
                DataEnvio = DateTimeExtension.HorarioBrasilia(),
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new InformativoPerfil()
            {
                InformativoId = INFORME_ID_2,
                CodigoPerfil = PERFIL_ADM_UE,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var filtro = new InformeFiltroDto()
            {
                DreId = DRE_ID_1,
                UeId = UE_ID_1
            };

            var useCase = ServiceProvider.GetService<IObterInformesPorFiltroUseCase>();
            var resultado = await useCase.Executar(filtro);

            resultado.ShouldNotBeNull();
            resultado.TotalPaginas.ShouldBe(1);
            resultado.TotalRegistros.ShouldBe(1);
            resultado.Items.ShouldNotBeNull();
            resultado.Items.Count().ShouldBe(1);
            var item2 = resultado.Items.FirstOrDefault(item => item.Id == 2);
            item2.DreNome.ShouldBe(DRE_NOME_1);
            item2.UeNome.ShouldBe($"{TipoEscola.EMEF.ShortName()} {UE_NOME_1}");
            item2.Titulo.ShouldBe("informe 2");
            item2.DataEnvio.ShouldBe(DateTimeExtension.HorarioBrasilia().ToString("dd/MM/yyyy"));
            item2.EnviadoPor.ShouldBe($"{SISTEMA_NOME} ({SISTEMA_CODIGO_RF})");
        }

        [Fact(DisplayName = "Informes - Obter informes filtros titulo")]
        public async Task Ao_obter_informes_dre_filtros()
        {
            await CriarDadosBase();

            await InserirNaBase(new Informativo()
            {
                Titulo = "informe 1",
                Texto = "teste 1",
                DataEnvio = DateTimeExtension.HorarioBrasilia(),
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

            await InserirNaBase(new Informativo()
            {
                DreId = DRE_ID_1,
                UeId = UE_ID_1,
                Titulo = "informe 2",
                Texto = "teste 2",
                DataEnvio = DateTimeExtension.HorarioBrasilia(),
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new InformativoPerfil()
            {
                InformativoId = INFORME_ID_2,
                CodigoPerfil = PERFIL_ADM_UE,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var filtro = new InformeFiltroDto()
            {
                Titulo = "informe"
            };

            var useCase = ServiceProvider.GetService<IObterInformesPorFiltroUseCase>();
            var resultado = await useCase.Executar(filtro);

            resultado.ShouldNotBeNull();
            resultado.TotalPaginas.ShouldBe(1);
            resultado.TotalRegistros.ShouldBe(2);
            resultado.Items.ShouldNotBeNull();
            resultado.Items.Count().ShouldBe(2);
            var item1 = resultado.Items.FirstOrDefault(item => item.Id == 1);
            item1.DreNome.ShouldBe("Todas");
            item1.UeNome.ShouldBe("Todas");
            item1.Titulo.ShouldBe("informe 1");
            item1.DataEnvio.ShouldBe(DateTimeExtension.HorarioBrasilia().ToString("dd/MM/yyyy"));
            item1.EnviadoPor.ShouldBe($"{SISTEMA_NOME} ({SISTEMA_CODIGO_RF})");
            var item2 = resultado.Items.FirstOrDefault(item => item.Id == 2);
            item2.DreNome.ShouldBe(DRE_NOME_1);
            item2.UeNome.ShouldBe($"{TipoEscola.EMEF.ShortName()} {UE_NOME_1}");
            item2.Titulo.ShouldBe("informe 2");
            item2.DataEnvio.ShouldBe(DateTimeExtension.HorarioBrasilia().ToString("dd/MM/yyyy"));
            item2.EnviadoPor.ShouldBe($"{SISTEMA_NOME} ({SISTEMA_CODIGO_RF})");
        }

        [Fact(DisplayName = "Informes - Obter informes filtros data envio")]
        public async Task Ao_obter_informes_data_envio_filtros()
        {
            await CriarDadosBase();

            await InserirNaBase(new Informativo()
            {
                Titulo = "informe 1",
                Texto = "teste 1",
                DataEnvio = DateTimeExtension.HorarioBrasilia(),
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

            await InserirNaBase(new Informativo()
            {
                DreId = DRE_ID_1,
                UeId = UE_ID_1,
                Titulo = "informe 2",
                Texto = "teste 2",
                DataEnvio = DateTimeExtension.HorarioBrasilia(),
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new InformativoPerfil()
            {
                InformativoId = INFORME_ID_2,
                CodigoPerfil = PERFIL_ADM_UE,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var filtro = new InformeFiltroDto()
            {
                DataEnvioInicio = DateTimeExtension.HorarioBrasilia(),
                DataEnvioFim = DateTimeExtension.HorarioBrasilia().AddMonths(1)
            };

            var useCase = ServiceProvider.GetService<IObterInformesPorFiltroUseCase>();
            var resultado = await useCase.Executar(filtro);

            resultado.ShouldNotBeNull();
            resultado.TotalPaginas.ShouldBe(1);
            resultado.TotalRegistros.ShouldBe(2);
            resultado.Items.ShouldNotBeNull();
            resultado.Items.Count().ShouldBe(2);
            var item1 = resultado.Items.FirstOrDefault(item => item.Id == 1);
            item1.DreNome.ShouldBe("Todas");
            item1.UeNome.ShouldBe("Todas");
            item1.Titulo.ShouldBe("informe 1");
            item1.DataEnvio.ShouldBe(DateTimeExtension.HorarioBrasilia().ToString("dd/MM/yyyy"));
            item1.EnviadoPor.ShouldBe($"{SISTEMA_NOME} ({SISTEMA_CODIGO_RF})");
            var item2 = resultado.Items.FirstOrDefault(item => item.Id == 2);
            item2.DreNome.ShouldBe(DRE_NOME_1);
            item2.UeNome.ShouldBe($"{TipoEscola.EMEF.ShortName()} {UE_NOME_1}");
            item2.Titulo.ShouldBe("informe 2");
            item2.DataEnvio.ShouldBe(DateTimeExtension.HorarioBrasilia().ToString("dd/MM/yyyy"));
            item2.EnviadoPor.ShouldBe($"{SISTEMA_NOME} ({SISTEMA_CODIGO_RF})");
        }

        [Fact(DisplayName = "Informes - Obter informes filtros perfil")]
        public async Task Ao_obter_informes_filtros_perfil()
        {
            await CriarDadosBase();

            await InserirNaBase(new Informativo()
            {
                Titulo = "informe 1",
                Texto = "teste 1",
                DataEnvio = DateTimeExtension.HorarioBrasilia(),
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

            await InserirNaBase(new Informativo()
            {
                DreId = DRE_ID_1,
                UeId = UE_ID_1,
                Titulo = "informe 2",
                Texto = "teste 2",
                DataEnvio = DateTimeExtension.HorarioBrasilia(),
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new InformativoPerfil()
            {
                InformativoId = INFORME_ID_2,
                CodigoPerfil = PERFIL_ADM_UE,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var filtro = new InformeFiltroDto()
            {
                Perfis = new List<long> { PERFIL_ADM_UE },
            };

            var useCase = ServiceProvider.GetService<IObterInformesPorFiltroUseCase>();
            var resultado = await useCase.Executar(filtro);

            resultado.ShouldNotBeNull();
            resultado.TotalPaginas.ShouldBe(1);
            resultado.TotalRegistros.ShouldBe(1);
            resultado.Items.ShouldNotBeNull();
            resultado.Items.Count().ShouldBe(1);
            var item2 = resultado.Items.FirstOrDefault(item => item.Id == 2);
            item2.DreNome.ShouldBe(DRE_NOME_1);
            item2.UeNome.ShouldBe($"{TipoEscola.EMEF.ShortName()} {UE_NOME_1}");
            item2.Titulo.ShouldBe("informe 2");
            item2.DataEnvio.ShouldBe(DateTimeExtension.HorarioBrasilia().ToString("dd/MM/yyyy"));
            item2.EnviadoPor.ShouldBe($"{SISTEMA_NOME} ({SISTEMA_CODIGO_RF})");
        }
    }
}
