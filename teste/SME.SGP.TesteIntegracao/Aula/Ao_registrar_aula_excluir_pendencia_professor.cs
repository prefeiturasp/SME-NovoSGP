using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Aula
{
    public class Ao_registrar_aula_excluir_pendencia_professor : AulaTeste
    {
        public Ao_registrar_aula_excluir_pendencia_professor(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Registrar aula única com exclusão de pendência de professor com componente sem aula")]
        public async Task Ao_registrar_aula_unica_excluir_pendencia_professor_componente_sem_aula()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_08_07, BIMESTRE_2);

            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);

            await InserirNaBase(new Pendencia()
            {
                Id = 1,
                Tipo = TipoPendencia.ComponenteSemAula,
                Descricao = "Pendência professor componente sem aula",
                Titulo = "Pendência professor componente sem aula",
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new System.DateTime(DateTimeExtension.HorarioBrasilia().Year, 06, 08)
            });

            await InserirNaBase(new PendenciaProfessor()
            {
                PendenciaId = 1,
                PeriodoEscolarId = 1,
                TurmaId = TURMA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                ProfessorRf = USUARIO_PROFESSOR_LOGIN_2222222
            });

            await InserirAulaUseCaseComValidacaoBasica(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_02_05);

            var pendencias = ObterTodos<PendenciaProfessor>();

            pendencias.ShouldNotBeNull();

            pendencias.Any().ShouldBeFalse();
        }

        [Fact]
        public async Task Ao_registrar_aula_recorrente_excluir_pendencia_professor_componente_sem_aula()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_03_01, DATA_28_04, BIMESTRE_1, false);

            await CriarPeriodoEscolarEPeriodoReabertura();

            await InserirNaBase(new Pendencia()
            {
                Id = 1,
                Tipo = TipoPendencia.ComponenteSemAula,
                Descricao = "Pendência professor componente sem aula",
                Titulo = "Pendência professor componente sem aula",
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new System.DateTime(DateTimeExtension.HorarioBrasilia().Year, 06, 08)
            });

            await InserirNaBase(new PendenciaProfessor()
            {
                PendenciaId = 1,
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_1,
                TurmaId = TURMA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                ProfessorRf = USUARIO_PROFESSOR_LOGIN_2222222
            });

            await InserirNaBase(new PendenciaProfessor()
            {
                PendenciaId = 1,
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_2,
                TurmaId = TURMA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                ProfessorRf = USUARIO_PROFESSOR_LOGIN_2222222
            });

            await InserirNaBase(new PendenciaProfessor()
            {
                PendenciaId = 1,
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_3,
                TurmaId = TURMA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                ProfessorRf = USUARIO_PROFESSOR_LOGIN_2222222
            });

            await InserirNaBase(new PendenciaProfessor()
            {
                PendenciaId = 1,
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_4,
                TurmaId = TURMA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                ProfessorRf = USUARIO_PROFESSOR_LOGIN_2222222
            });

            await InserirAulaUseCaseSemValidacaoBasica(TipoAula.Normal, RecorrenciaAula.RepetirTodosBimestres, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_03_01, false, TIPO_CALENDARIO_1);

            var pendencias = ObterTodos<PendenciaProfessor>();

            pendencias.ShouldNotBeNull();

            pendencias.Any().ShouldBeFalse();
        }

        private async Task CriarPeriodoEscolarEPeriodoReabertura()
        {
            await CriarPeriodoEscolar(DATA_03_01, DATA_28_04, BIMESTRE_1, TIPO_CALENDARIO_1);

            await CriarPeriodoEscolar(DATA_02_05, DATA_08_07, BIMESTRE_2, TIPO_CALENDARIO_1);

            await CriarPeriodoEscolar(DATA_25_07, DATA_30_09, BIMESTRE_3, TIPO_CALENDARIO_1);

            await CriarPeriodoEscolar(DATA_03_10, DATA_22_12, BIMESTRE_4, TIPO_CALENDARIO_1);

            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);
        }
    }
}
