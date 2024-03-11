
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.Aula.DiarioBordo.ServicosFakes;
using SME.SGP.TesteIntegracao.DiarioBordo;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Aula.DiarioBordo
{
    public class Ao_salvar_notificacao_devolutiva : DiarioBordoTesteBase
    {
        public Ao_salvar_notificacao_devolutiva(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        { 
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<SolicitaRelatorioDevolutivasCommand, Guid>),
                            typeof(SolicitaRelatorioDevolutivasCommandFake), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery, IEnumerable<ComponenteCurricularEol>>),
                typeof(ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQueryHandlerFakeServicoEol), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Ao_salvar_notificacao_devolutiva_unificada()
        {
            var filtroDiarioBordo = new FiltroDiarioBordoDto
            {
                ComponenteCurricularId = COMPONENTE_CURRICULAR_512,
                DataAulaDiarioBordo = DateTimeExtension.HorarioBrasilia().Date
            };

            await CriarDadosBasicos(filtroDiarioBordo, false);

            await CriarAula(filtroDiarioBordo.DataAulaDiarioBordo, RecorrenciaAula.AulaUnica,
                TipoAula.Normal,
                USUARIO_PROFESSOR_CODIGO_RF_2222222,
                TURMA_CODIGO_1, UE_CODIGO_1,
                COMPONENTE_CURRICULAR_513.ToString(), TIPO_CALENDARIO_1);

            await InserirNaBase(new Dominio.DiarioBordo()
            {
                Id = DIARIO_BORDO_ID_1,
                AulaId = AULA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_512,
                TurmaId = TURMA_ID_1,
                DevolutivaId = null,
                Planejamento = "Planejado 512",
                Excluido = false,
                InseridoCJ = false,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = "Sistema",
                CriadoRF = USUARIO_PROFESSOR_CODIGO_RF_1111111
            });

            await InserirNaBase(new Dominio.DiarioBordo()
            {
                Id = DIARIO_BORDO_ID_2,
                AulaId = AULA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_513,
                TurmaId = TURMA_ID_1,
                DevolutivaId = null,
                Planejamento = "Planejado 513",
                Excluido = false,
                InseridoCJ = false,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = "Sistema",
                CriadoRF = USUARIO_PROFESSOR_CODIGO_RF_1111111
            });

            await InserirNaBase(new Dominio.Devolutiva()
            {
                CodigoComponenteCurricular = COMPONENTE_CURRICULAR_512,
                Descricao = "Devolutiva",
                PeriodoInicio = DateTimeExtension.HorarioBrasilia(),
                PeriodoFim = DateTimeExtension.HorarioBrasilia().AddDays(5),
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = "Sistema",
                CriadoRF = USUARIO_PROFESSOR_CODIGO_RF_1111111
            });

            var dto = new SalvarNotificacaoDevolutivaDto(TURMA_ID_1, USUARIO_CP_NOME_3333333, USUARIO_CP_CODIGO_RF_3333333, 1); 
            var mensagem = new MensagemRabbit(JsonSerializer.Serialize(dto));
            var useCase = ServiceProvider.GetService<ISalvarNotificacaoDevolutivaUseCase>();

            await useCase.Executar(mensagem);

            var notificacoes = ObterTodos<Notificacao>();
            notificacoes.ShouldNotBeNull();
            notificacoes.Count.ShouldBe(2);
            var notificacao1 = notificacoes.Find(item => item.UsuarioId == USUARIO_ID_1);
            notificacao1.ShouldNotBeNull();
            notificacao1.Titulo.ShouldBe("Devolutiva do Diário de bordo da turma Turma Nome 1 - REGÊNCIA INFANTIL EMEI 2H"); 
            var notificacao2 = notificacoes.Find(item => item.UsuarioId == USUARIO_ID_2);
            notificacao2.ShouldNotBeNull();
            notificacao2.Titulo.ShouldBe("Devolutiva do Diário de bordo da turma Turma Nome 1 - REGÊNCIA INFANTIL EMEI 4H");
        }

        [Fact]
        public async Task Ao_salvar_notificacao_devolutiva_unica()
        {
            var filtroDiarioBordo = new FiltroDiarioBordoDto
            {
                ComponenteCurricularId = COMPONENTE_CURRICULAR_512,
                DataAulaDiarioBordo = DateTimeExtension.HorarioBrasilia().Date
            };

            await CriarDadosBasicos(filtroDiarioBordo, false);

            await InserirNaBase(new Dominio.Turma
            {
                UeId = 1,
                Ano = TURMA_ANO_2,
                CodigoTurma = TURMA_CODIGO_4,
                Historica = false,
                ModalidadeCodigo = Modalidade.EducacaoInfantil,
                AnoLetivo = 2023,
                Semestre = SEMESTRE_1,
                Nome = TURMA_NOME_1,
                TipoTurma = TipoTurma.Regular
            });

            await CriarAula(filtroDiarioBordo.DataAulaDiarioBordo, RecorrenciaAula.AulaUnica,
                TipoAula.Normal,
                USUARIO_PROFESSOR_CODIGO_RF_2222222,
                TURMA_CODIGO_4, UE_CODIGO_1,
                COMPONENTE_CURRICULAR_513.ToString(), TIPO_CALENDARIO_1);

            await InserirNaBase(new Dominio.DiarioBordo()
            {
                Id = DIARIO_BORDO_ID_1,
                AulaId = AULA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_512,
                TurmaId = TURMA_ID_4,
                DevolutivaId = null,
                Planejamento = "Planejado 512",
                Excluido = false,
                InseridoCJ = false,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = "Sistema",
                CriadoRF = USUARIO_PROFESSOR_CODIGO_RF_1111111
            });

            await InserirNaBase(new Dominio.DiarioBordo()
            {
                Id = DIARIO_BORDO_ID_2,
                AulaId = AULA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_513,
                TurmaId = TURMA_ID_4,
                DevolutivaId = null,
                Planejamento = "Planejado 513",
                Excluido = false,
                InseridoCJ = false,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = "Sistema",
                CriadoRF = USUARIO_PROFESSOR_CODIGO_RF_1111111
            });

            await InserirNaBase(new Dominio.Devolutiva()
            {
                CodigoComponenteCurricular = COMPONENTE_CURRICULAR_512,
                Descricao = "Devolutiva",
                PeriodoInicio = DateTimeExtension.HorarioBrasilia(),
                PeriodoFim = DateTimeExtension.HorarioBrasilia().AddDays(5),
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = "Sistema",
                CriadoRF = USUARIO_PROFESSOR_CODIGO_RF_1111111
            });

            var dto = new SalvarNotificacaoDevolutivaDto(TURMA_ID_4, USUARIO_CP_NOME_3333333, USUARIO_CP_CODIGO_RF_3333333, 1);
            var mensagem = new MensagemRabbit(JsonSerializer.Serialize(dto));
            var useCase = ServiceProvider.GetService<ISalvarNotificacaoDevolutivaUseCase>();

            await useCase.Executar(mensagem);

            var notificacoes = ObterTodos<Notificacao>();
            notificacoes.ShouldNotBeNull();
            notificacoes.Count.ShouldBe(1);
            var notificacao1 = notificacoes.Find(item => item.UsuarioId == USUARIO_ID_2);
            notificacao1.ShouldNotBeNull();
            notificacao1.Titulo.ShouldBe("Devolutiva do Diário de bordo da turma Turma Nome 1 - REGÊNCIA INFANTIL EMEI 4H");
        }
    }
}
