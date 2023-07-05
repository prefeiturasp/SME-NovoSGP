using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.RelatorioAcompanhamentoAprendizagem
{
    public class Ao_validar_percurso : RelatorioAcompanhamentoAprendizagemTesteBase
    {
        public Ao_validar_percurso(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Validar RAA com percurso coletivo")]
        public async Task Ao_validar_com_percurso_coletivo()
        {
            await CriarDadosBasicos();
            await CriarPeriodoEscolarCustomizadoSegundoBimestre(true);
            await CriaAcompanhamentoColetivo();

            var useCase = ServiceProvider.GetService<IObterValidacaoPercusoRAAUseCase>();
            var inconsistencia = await useCase.Executar(new FiltroInconsistenciaPercursoRAADto(TURMA_ID_1, 1));

            inconsistencia.ShouldNotBeNull();
            inconsistencia.MensagemInsconsistenciaPercursoColetivo.ShouldBe(string.Empty); 
        }

        [Fact(DisplayName = "Validar RAA sem percurso coletivo")]
        public async Task Ao_validar_sem_percurso_coletivo()
        {
            await CriarDadosBasicos();
            await CriarPeriodoEscolarCustomizadoSegundoBimestre(true);
            await CriaAcomanhamentoIndividual();

            var useCase = ServiceProvider.GetService<IObterValidacaoPercusoRAAUseCase>();
            var inconsistencia = await useCase.Executar(new FiltroInconsistenciaPercursoRAADto(TURMA_ID_1, 1));

            inconsistencia.ShouldNotBeNull();
            inconsistencia.MensagemInsconsistenciaPercursoColetivo.ShouldBe(MensagemNegocioAcomponhamentoAluno.AUSENCIA_PREENCHIMENTO_PERCUSO_COLETIVO);
        }

        [Fact(DisplayName = "Validar RAA sem percurso individual")]
        public async Task Ao_validar_sem_percurso_individual()
        {
            await CriarDadosBasicos();
            await CriarPeriodoEscolarCustomizadoSegundoBimestre(true);
            await CriaAcomanhamentoIndividual();

            var useCase = ServiceProvider.GetService<IObterValidacaoPercusoRAAUseCase>();
            var inconsistencia = await useCase.Executar(new FiltroInconsistenciaPercursoRAADto(TURMA_ID_1, 1));

            inconsistencia.ShouldNotBeNull();
            inconsistencia.InconsistenciaPercursoIndividual.ShouldNotBeNull();
            var alunosInconsistentes = inconsistencia.InconsistenciaPercursoIndividual.AlunosComInconsistenciaPercursoIndividualRAA;
            alunosInconsistentes.ShouldNotBeNull();
            alunosInconsistentes.ToList().Exists(aluno => aluno.AlunoCodigo == ALUNO_CODIGO_3).ShouldBeTrue();
            alunosInconsistentes.ToList().Exists(aluno => aluno.AlunoCodigo == ALUNO_CODIGO_4).ShouldBeTrue();
            alunosInconsistentes.ToList().Exists(aluno => aluno.AlunoCodigo == ALUNO_CODIGO_1).ShouldBeFalse();
            alunosInconsistentes.ToList().Exists(aluno => aluno.AlunoCodigo == ALUNO_CODIGO_2).ShouldBeFalse();
        }

        [Fact(DisplayName = "Validar RAA todos alunos sem percurso individual")]
        public async Task Ao_validar_todos_alunos_sem_percurso_individual()
        {
            await CriarDadosBasicos();
            await CriarPeriodoEscolarCustomizadoSegundoBimestre(true);

            var useCase = ServiceProvider.GetService<IObterValidacaoPercusoRAAUseCase>();
            var inconsistencia = await useCase.Executar(new FiltroInconsistenciaPercursoRAADto(TURMA_ID_1, 1));

            inconsistencia.ShouldNotBeNull();
            inconsistencia.InconsistenciaPercursoIndividual.ShouldNotBeNull();
            inconsistencia.InconsistenciaPercursoIndividual.MensagemInsconsistencia.ShouldBe(MensagemNegocioAcomponhamentoAluno.NENHUMA_CRIANCAO_POSSUI_PERCURSO_INDIVIDUAL);
            inconsistencia.InconsistenciaPercursoIndividual.AlunosComInconsistenciaPercursoIndividualRAA.ShouldBeNull();
        }

        private async Task CriaAcomanhamentoIndividual()
        {
            await InserirNaBase(new AcompanhamentoAluno()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new AcompanhamentoAlunoSemestre()
            {
                PercursoIndividual = "teste",
                Semestre = 1,
                AcompanhamentoAlunoId = 1,
                Observacoes = "observações",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new AcompanhamentoAluno()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = ALUNO_CODIGO_2,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new AcompanhamentoAlunoSemestre()
            {
                PercursoIndividual = "teste",
                Semestre = 1,
                AcompanhamentoAlunoId = 2,
                Observacoes = "observações",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });
        }

        private async Task CriaAcompanhamentoColetivo()
        {
            await InserirNaBase(new AcompanhamentoTurma()
            {
                TurmaId = TURMA_ID_1,
                ApanhadoGeral = "Acompanhamento",
                Semestre = 1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });
        }
    }
}
