using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.PendenciaComponenteSemAula.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.PendenciaComponenteSemAula
{
    public class Ao_gerar_pendencia_componente_sem_aula : TesteBaseComuns
    {
        const int DIAS_APOS_INICIO_PERIODO = 15;
        const int DIAS_APOS_INICIO_PERIODO_DENTRO_INTERVALO = 50;
        const int PERIODO_INICIO = -45;
        const int PERIODO_FIM = +30;
        public Ao_gerar_pendencia_componente_sem_aula(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaItinerarioEnsinoMedioQuery, IEnumerable<TurmaItinerarioEnsinoMedioDto>>), typeof(ObterTurmaItinerarioEnsinoMedioQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEOLPorTurmasCodigoQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesCurricularesEOLPorTurmasCodigoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterProfessorTitularPorTurmaEComponenteCurricularQuery, ProfessorTitularDisciplinaEol>), typeof(ObterProfessorTitularPorTurmaEComponenteCurricularQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Nao_deve_chamar_fila_para_gerar_pendencia_turma_programa()
        {
            bool retorno = await PendenciaTurmaComponenteSemAulaPorUeUseCase(TipoTurma.Programa, ANO_6);
            retorno.ShouldBeFalse();
        }

        [Fact]
        public async Task Nao_deve_chamar_fila_para_gerar_pendencia_turma_ciclo_alfabetizacao()
        {
            bool retorno = await PendenciaTurmaComponenteSemAulaPorUeUseCase(TipoTurma.Regular, ANO_1);
            retorno.ShouldBeFalse();
        }

        [Fact]
        public async Task Deve_chamar_fila_para_gerar_pendencia_turma_regular()
        {
            bool retorno = await PendenciaTurmaComponenteSemAulaPorUeUseCase(TipoTurma.Regular, ANO_6);
            retorno.ShouldBeTrue();
        }

        private async Task<bool> PendenciaTurmaComponenteSemAulaPorUeUseCase(TipoTurma tipoTurma, string anoTurma)
        {
            var pendenciaTurmaComponenteSemAulasPorUeUseCase = ServiceProvider.GetService<IPendenciaTurmaComponenteSemAulasPorUeUseCase>();

            await CriarDreUe(DRE_CODIGO_1, UE_CODIGO_1);

            await InserirNaBase(new Dominio.Turma
            {
                Nome = "Turma teste",
                CodigoTurma = TURMA_CODIGO_1,
                Ano = anoTurma,
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                TipoTurma = tipoTurma,
                ModalidadeCodigo = Modalidade.Fundamental,
                UeId = DRE_ID_1
            });

            await InserirNaBase("tipo_ciclo", new string[4] { "descricao", "criado_em", "criado_por", "criado_rf" }, new string[4] { "'Alfabetização'", "'2022-06-07'", "'1'", "'1'" });
            await InserirNaBase("tipo_ciclo", new string[4] { "descricao", "criado_em", "criado_por", "criado_rf" }, new string[4] { "'Interdiciplinar'", "'2022-06-07'", "'1'", "'1'" });
            await InserirNaBase("tipo_ciclo_ano", new string[3] { "tipo_ciclo_id", "modalidade", "ano" }, new string[3] { "1", "5", "'1'" });
            await InserirNaBase("tipo_ciclo_ano", new string[3] { "tipo_ciclo_id", "modalidade", "ano" }, new string[3] { "2", "5", "'6'" });

            return await pendenciaTurmaComponenteSemAulasPorUeUseCase.Executar(new MensagemRabbit(JsonSerializer.Serialize(new DreUeDto(DRE_ID_1, UE_ID_1))));
        }

        [Fact]
        public async Task Nao_deve_gerar_pendencia_quando_data_atual_estiver_fora_do_periodo_escolar()
        {
            var pendenciaTurmaComponenteSemAulasUseCase = ServiceProvider.GetService<IPendenciaTurmaComponenteSemAulasUseCase>();

            await CriarDadosBasicos(DIAS_APOS_INICIO_PERIODO, -30, -30);

            var retorno = await pendenciaTurmaComponenteSemAulasUseCase.Executar(new MensagemRabbit(JsonSerializer.Serialize(new TurmaDTO { TurmaCodigo = TURMA_CODIGO_1, TurmaId = TURMA_ID_1 })));
            retorno.ShouldBeFalse();
        }

        [Fact]
        public async Task Nao_deve_gerar_pendencia_antes_do_parametro_dias_apos_inicio_periodo()
        {
            var pendenciaTurmaComponenteSemAulasUseCase = ServiceProvider.GetService<IPendenciaTurmaComponenteSemAulasUseCase>();

            await CriarDadosBasicos(DIAS_APOS_INICIO_PERIODO_DENTRO_INTERVALO, PERIODO_INICIO, PERIODO_FIM);

            var retorno = await pendenciaTurmaComponenteSemAulasUseCase.Executar(new MensagemRabbit(JsonSerializer.Serialize(new TurmaDTO { TurmaCodigo = TURMA_CODIGO_1, TurmaId = TURMA_ID_1 })));
            retorno.ShouldBeFalse();
        }

        [Fact]
        public async Task Nao_deve_gerar_pendencia_para_turmas_com_regencia_de_classe()
        {
            var pendenciaTurmaComponenteSemAulasUseCase = ServiceProvider.GetService<IPendenciaTurmaComponenteSemAulasUseCase>();

            await CriarDadosBasicos(DIAS_APOS_INICIO_PERIODO, PERIODO_INICIO, PERIODO_FIM);

            await InserirNaBase(new Dominio.Turma
            {
                Nome = TURMA_ANO_3,
                CodigoTurma = TURMA_CODIGO_2,
                Ano = ANO_6,
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                TipoTurma = TipoTurma.Regular,
                ModalidadeCodigo = Modalidade.Fundamental,
                UeId = DRE_ID_1
            });

            var retorno = await pendenciaTurmaComponenteSemAulasUseCase.Executar(new MensagemRabbit(JsonSerializer.Serialize(new TurmaDTO { TurmaCodigo = TURMA_CODIGO_2, TurmaId = TURMA_ID_2 })));
            retorno.ShouldBeFalse();
        }

        [Fact]
        public async Task Nao_deve_gerar_pendencia_para_turmas_e_componente_sem_professor_titular()
        {
            var pendenciaTurmaComponenteSemAulasUseCase = ServiceProvider.GetService<IPendenciaTurmaComponenteSemAulasUseCase>();

            await CriarDadosBasicos(DIAS_APOS_INICIO_PERIODO, PERIODO_INICIO, PERIODO_FIM);

            await InserirNaBase(new Dominio.Turma
            {
                Nome = TURMA_ANO_4,
                CodigoTurma = TURMA_CODIGO_3,
                Ano = ANO_6,
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                TipoTurma = TipoTurma.Regular,
                ModalidadeCodigo = Modalidade.Fundamental,
                UeId = DRE_ID_1
            });

            var retorno = await pendenciaTurmaComponenteSemAulasUseCase.Executar(new MensagemRabbit(JsonSerializer.Serialize(new TurmaDTO { TurmaCodigo = TURMA_CODIGO_3, TurmaId = TURMA_ID_2 })));
            retorno.ShouldBeFalse();
        }

        [Fact]
        public async Task Nao_deve_gerar_pendencia_para_turmas_que_possui_aula_no_periodo()
        {
            var pendenciaTurmaComponenteSemAulasUseCase = ServiceProvider.GetService<IPendenciaTurmaComponenteSemAulasUseCase>();

            await CriarDadosBasicos(DIAS_APOS_INICIO_PERIODO_DENTRO_INTERVALO, PERIODO_INICIO, PERIODO_FIM);
            await CriarAula(DateTimeExtension.HorarioBrasilia(), RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_CODIGO_RF_1111111, TURMA_CODIGO_1, UE_CODIGO_1, COMPONENTE_LINGUA_PORTUGUESA_ID_138, TIPO_CALENDARIO_1);

            var retorno = await pendenciaTurmaComponenteSemAulasUseCase.Executar(new MensagemRabbit(JsonSerializer.Serialize(new TurmaDTO { TurmaCodigo = TURMA_CODIGO_1, TurmaId = TURMA_ID_1 })));
            retorno.ShouldBeFalse();
        }

        [Fact]
        public async Task Deve_gerar_pendencia_para_turmas_que_nao_possui_aula_no_periodo()
        {
            var pendenciaTurmaComponenteSemAulasUseCase = ServiceProvider.GetService<IPendenciaTurmaComponenteSemAulasUseCase>();

            await CriarDadosBasicos(DIAS_APOS_INICIO_PERIODO, PERIODO_INICIO, PERIODO_FIM);

            var retorno = await pendenciaTurmaComponenteSemAulasUseCase.Executar(new MensagemRabbit(JsonSerializer.Serialize(new TurmaDTO { TurmaCodigo = TURMA_CODIGO_1, TurmaId = TURMA_ID_1 })));
            retorno.ShouldBeTrue();

            var pendencias = ObterTodos<Pendencia>();
            var pendencia = pendencias.FirstOrDefault();
            pendencia.TurmaId.ShouldBe(TURMA_ID_1);

            var pendenciasProfessores = ObterTodos<PendenciaProfessor>();
            var pendenciaProfessor = pendenciasProfessores.FirstOrDefault(t => t.PendenciaId == pendencia.Id);
            pendenciaProfessor.ComponenteCurricularId.ShouldBe(long.Parse(COMPONENTE_LINGUA_PORTUGUESA_ID_138));
        }

        private async Task CriarDadosBasicos(int diasAposInicioPeriodo, int diasPeriodoInicial, int diasPeriodoFinal)
        {
            await CriarDreUe(DRE_CODIGO_1, UE_CODIGO_1);

            await InserirNaBase(new Dominio.Turma
            {
                Nome = TURMA_ANO_2,
                CodigoTurma = TURMA_CODIGO_1,
                Ano = ANO_6,
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                TipoTurma = TipoTurma.Regular,
                ModalidadeCodigo = Modalidade.Fundamental,
                UeId = DRE_ID_1
            });

            await CriarTipoCalendario(ModalidadeTipoCalendario.FundamentalMedio);

            await CriarComponenteCurricular();

            await InserirNaBase(new ParametrosSistema
            {
                Id = 1,
                Tipo = TipoParametroSistema.DiasAposInicioPeriodoLetivoComponenteSemAula,
                Ativo = true,
                CriadoPor = "",
                CriadoRF = "",
                Valor = diasAposInicioPeriodo.ToString(),
                Nome = "DiasAposInicioPeriodoLetivoComponenteSemAula",
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                Descricao = "dias após o inicio do período para gerar pendencias de componentes sem aula"
            });

            var dataReferencia = DateTime.Now;
            await CriarPeriodoEscolar(dataReferencia.AddDays(diasPeriodoInicial), dataReferencia.AddDays(diasPeriodoFinal), BIMESTRE_1, TIPO_CALENDARIO_1);
        }
    }
}
