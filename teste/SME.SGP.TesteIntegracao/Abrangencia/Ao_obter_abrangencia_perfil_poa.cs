using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.Abrangencia.Base;
using SME.SGP.TesteIntegracao.Abrangencia.Fake;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace SME.SGP.TesteIntegracao.Abrangencia
{
    public class Ao_obter_abrangencia_perfil_poa : AbrangenciaBase
    {
        public Ao_obter_abrangencia_perfil_poa(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAbrangenciaCompactaVigenteEolPorLoginEPerfilQuery, AbrangenciaCompactaVigenteRetornoEOLDTO>), typeof(ObterAbrangenciaCompactaVigenteEolPorLoginEPerfilQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Abrangência - Obter modalidades de abrangência para perfil POA")]
        public async Task Ao_obter_modalidades_abrangencia_perfil_poa()
        {
            var filtro = new FiltroTesteDto()
            {
                Perfil = ObterPerfilPOA_Portugues(),
                AnoTurma = ANO_7,
                Modalidade = Modalidade.Medio,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            };

            await CriarDadosBase(filtro);

            await InserirNaBase(new Dominio.Abrangencia()
            {
                UsuarioId = USUARIO_ID_1,
                Perfil = Guid.Parse(PerfilUsuario.POA_LINGUA_PORTUGUESA.Name()),
                TurmaId = TURMA_ID_1
            });

            var useCase = ServiceProvider.GetService<IObterModalidadesPorAnoUseCase>();

            var modalidades = await useCase.Executar(DateTime.Now.Year, false, false);

            modalidades.ShouldNotBeNull();

            modalidades.Count().ShouldBe(1);
            modalidades.First().Id.ShouldBe((int)Modalidade.Medio);
        }

        [Fact(DisplayName = "Abrangência - Obter anos letivo da abrangência para perfil POA")]
        public async Task Ao_obter_anos_letivos_abrangencia_perfil_poa()
        {
            var filtro = new FiltroTesteDto()
            {
                Perfil = ObterPerfilPOA_Portugues(),
                AnoTurma = ANO_7,
                Modalidade = Modalidade.Medio,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            };

            await CriarDadosBase(filtro);

            var consulta = ServiceProvider.GetService<IConsultasAbrangencia>();

            var anos = await consulta.ObterAnosLetivos(false, 0);

            anos.ShouldNotBeNull();
            anos.Count().ShouldBe(1);
            anos.First().ShouldBe(DateTime.Now.Year);
        }

        [Fact(DisplayName = "Abrangência - Obter anos turmas por modalidade da abrangência para perfil POA")]
        public async Task Ao_obter_anos_turmas_modalidade_abrangencia_perfil_poa()
        {
            var filtro = new FiltroTesteDto()
            {
                Perfil = ObterPerfilPOA_Portugues(),
                AnoTurma = ANO_7,
                Modalidade = Modalidade.Medio,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            };

            await CriarDadosBase(filtro);

            var consulta = ServiceProvider.GetService<IConsultasAbrangencia>();

            await InserirNaBase(new Dominio.Abrangencia()
            {
                UsuarioId = USUARIO_ID_1,
                Perfil = Guid.Parse(PerfilUsuario.POA_LINGUA_PORTUGUESA.Name()),
                DreId = DRE_ID_1
            });

            var anos = await consulta.ObterAnosTurmasPorUeModalidade(UE_CODIGO_1, filtro.Modalidade, false, DateTime.Now.Year);

            anos.ShouldNotBeNull();
            anos.Count().ShouldBe(1);
            anos.First().Valor.ShouldBe(ANO_7);
        }

        [Fact(DisplayName = "Abrangência - Obter dres da abrangência para perfil POA")]
        public async Task Ao_obter_dres_abrangencia_perfil_poa()
        {
            var filtro = new FiltroTesteDto()
            {
                Perfil = ObterPerfilPOA_Portugues(),
                AnoTurma = ANO_7,
                Modalidade = Modalidade.Medio,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            };

            await CriarDadosBase(filtro);

            await InserirNaBase(new Dominio.Abrangencia()
            {
                UsuarioId = USUARIO_ID_1,
                Perfil = Guid.Parse(PerfilUsuario.POA_LINGUA_PORTUGUESA.Name()),
                DreId = DRE_ID_1
            });

            var useCase = ServiceProvider.GetService<IObterAbrangenciaDresUseCase>();

            var dres = await useCase.Executar(filtro.Modalidade, 0, false, DateTime.Now.Year);
            dres.ShouldNotBeNull();
            dres.Count().ShouldBe(1);
            dres.First().Id.ShouldBe(DRE_ID_1);
        }

        [Fact(DisplayName = "Abrangência - Obter ues da abrangência para perfil POA")]
        public async Task Ao_obter_ues_abrangencia_perfil_poa()
        {
            var filtro = new FiltroTesteDto()
            {
                Perfil = ObterPerfilPOA_Portugues(),
                AnoTurma = ANO_7,
                Modalidade = Modalidade.Medio,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            };

            await CriarDadosBase(filtro);

            await InserirNaBase(new Dominio.Abrangencia()
            {
                UsuarioId = USUARIO_ID_1,
                Perfil = Guid.Parse(PerfilUsuario.POA_LINGUA_PORTUGUESA.Name()),
                DreId = DRE_ID_1
            });

            var useCase = ServiceProvider.GetService<IObterUEsPorDreUseCase>();
            var dto = new UEsPorDreDto()
            {
                CodigoDre = DRE_CODIGO_1,
                AnoLetivo = DateTime.Now.Year,
                Modalidade = Modalidade.Medio
            };
            var ues = await useCase.Executar(dto);
            ues.ShouldNotBeNull();
            ues.Count().ShouldBe(1);
            ues.First().Id.ShouldBe(UE_ID_1);
        }

        [Fact(DisplayName = "Abrangência - Obter turmas da abrangência para perfil POA")]
        public async Task Ao_obter_turmas_abrangencia_perfil_poa()
        {
            // ARRANGE - Configuração do cenário HTTP FAKE
            var turmasEolFake = new List<TurmaApiEolDto>
            {
                // Simula que a API do EOL retornou a turma que o SGP irá procurar.
                new TurmaApiEolDto { Codigo = int.Parse(TURMA_CODIGO_1) }
            };

            var jsonResposta = JsonConvert.SerializeObject(turmasEolFake);
            var conteudoHttp = new StringContent(jsonResposta, System.Text.Encoding.UTF8, "application/json");

            // A URL aqui é um prefixo. A implementação do handler usará 'StartsWith' para encontrá-la.
            var urlApiEol = ServiceProvider.GetService<IConfiguration>()["UrlApiEOL"];
            RegistradorDependenciasTeste.HttpHandlerFake.AdicionarCenario(urlApiEol, HttpStatusCode.OK, conteudoHttp);

            // ARRANGE - Continuação do setup do teste
            var filtro = new FiltroTesteDto()
            {
                Perfil = ObterPerfilPOA_Portugues(),
                AnoTurma = ANO_7,
                Modalidade = Modalidade.Medio,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            };

            await CriarDadosBase(filtro);

            await InserirNaBase(new Dominio.Abrangencia()
            {
                UsuarioId = USUARIO_ID_1,
                Perfil = Guid.Parse(PerfilUsuario.POA_LINGUA_PORTUGUESA.Name()),
                DreId = DRE_ID_1
            });

            var mediator = ServiceProvider.GetService<IMediator>();
            var turmas = await mediator.Send(new ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQuery(
                UE_CODIGO_1, filtro.Modalidade, 0, false, DateTime.Now.Year, null));
            turmas.ShouldNotBeNull();
            turmas.Count().ShouldBe(1);
            turmas.First().Id.ShouldBe(TURMA_ID_1);
        }

        [Fact(DisplayName = "Abrangência - Obter turmas não históricas abrangência")]
        public async Task Ao_obter_turmas_nao_historicas()
        {
            var filtro = new FiltroTesteDto()
            {
                Perfil = ObterPerfilPOA_Portugues(),
                AnoTurma = ANO_7,
                Modalidade = Modalidade.Medio,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            };

            await CriarDadosBase(filtro);

            await InserirNaBase(new Dominio.Abrangencia()
            {
                UsuarioId = USUARIO_ID_1,
                Perfil = Guid.Parse(PerfilUsuario.POA_LINGUA_PORTUGUESA.Name()),
                TurmaId = TURMA_ID_1
            });

            var useCase = ServiceProvider.GetService<IObterTurmasNaoHistoricasUseCase>();

            var turmas = await useCase.Executar();
            turmas.ShouldNotBeNull();
            turmas.Count().ShouldBe(1);
            turmas.First().Id.ShouldBe(TURMA_ID_1);
        }

        [Fact(DisplayName = "Abrangência - Obter abrangência por filtro para perfil POA")]
        public async Task Ao_obter_abrangencia_filtro_perfil_poa()
        {
            var filtro = new FiltroTesteDto()
            {
                Perfil = ObterPerfilPOA_Portugues(),
                AnoTurma = ANO_7,
                Modalidade = Modalidade.Medio,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            };

            await CriarDadosBase(filtro);

            await InserirNaBase(new Dominio.Abrangencia()
            {
                UsuarioId = USUARIO_ID_1,
                Perfil = Guid.Parse(PerfilUsuario.POA_LINGUA_PORTUGUESA.Name()),
                DreId = DRE_ID_1
            });

            var consulta = ServiceProvider.GetService<IConsultasAbrangencia>();
            var abrangencias = await consulta.ObterAbrangenciaPorfiltro(UE_NOME_1, false);

            abrangencias.ShouldNotBeNull();
            abrangencias.Count().ShouldBe(1);
            abrangencias.First().NomeUe.ShouldBe(UE_NOME_1);
        }

        [Fact(DisplayName = "Abrangência - Carregar abrangência para perfil POA")]
        public async Task Ao_carregar_abrangencia_perfil_poa()
        {
            var filtro = new FiltroTesteDto()
            {
                Perfil = ObterPerfilPOA_Portugues(),
                AnoTurma = ANO_7,
                Modalidade = Modalidade.Medio,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            };

            await CriarDadosBase(filtro);

            var useCase = ServiceProvider.GetService<ICarregarAbrangenciaUsusarioUseCase>();
            var perfil = Guid.Parse(PerfilUsuario.POA_LINGUA_PORTUGUESA.Name());
            await useCase.Executar(new UsuarioPerfilDto(USUARIO_PROFESSOR_LOGIN_2222222, perfil));

            var abrangencias = ObterTodos<Dominio.Abrangencia>();
            abrangencias.ShouldNotBeNull();
            abrangencias.Count().ShouldBe(1);
            abrangencias.First().TurmaId.ShouldBe(TURMA_ID_1);
            abrangencias.First().Perfil.ShouldBe(perfil);
        }
    }
}
