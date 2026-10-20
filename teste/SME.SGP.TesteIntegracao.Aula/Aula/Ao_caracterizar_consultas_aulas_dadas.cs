using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Aula
{
    public class Ao_caracterizar_consultas_aulas_dadas : AulaTeste
    {
        private const string PROFESSOR_1 = "1111111";
        private const string PROFESSOR_2 = "2222222";
        private readonly DateTime inicioPrimeiroBimestre;
        private readonly DateTime fimPrimeiroBimestre;
        private readonly DateTime inicioSegundoBimestre;
        private readonly DateTime fimSegundoBimestre;
        private readonly DateTime inicioTerceiroBimestre;
        private readonly DateTime fimTerceiroBimestre;
        private readonly DateTime inicioQuartoBimestre;
        private readonly DateTime fimQuartoBimestre;

        public Ao_caracterizar_consultas_aulas_dadas(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            var anoAtual = DateTime.Now.Year;
            inicioPrimeiroBimestre = new DateTime(anoAtual, 2, 1);
            fimPrimeiroBimestre = new DateTime(anoAtual, 4, 30);
            inicioSegundoBimestre = new DateTime(anoAtual, 5, 1);
            fimSegundoBimestre = new DateTime(anoAtual, 7, 31);
            inicioTerceiroBimestre = new DateTime(anoAtual, 8, 1);
            fimTerceiroBimestre = new DateTime(anoAtual, 9, 30);
            inicioQuartoBimestre = new DateTime(anoAtual, 10, 1);
            fimQuartoBimestre = new DateTime(anoAtual, 12, 31);
        }

        [Fact]
        public async Task Deve_contabilizar_aula_uma_unica_vez_quando_existirem_registros_de_frequencia_duplicados()
        {
            await CriarCenarioBasico();
            var aulaId = await CriarAulaComFrequencia(inicioPrimeiroBimestre.AddDays(10), PROFESSOR_1, 3);
            await CriarRegistroFrequencia(aulaId);

            var repositorio = ServiceProvider.GetRequiredService<IRepositorioAulaConsulta>();
            var total = await repositorio.ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolar(
                TURMA_CODIGO_1,
                new[] { COMPONENTE_CURRICULAR_PORTUGUES_ID_138 },
                1,
                new[] { 1L });

            total.ShouldBe(3);
        }

        [Fact]
        public async Task Deve_respeitar_periodos_professor_e_registros_excluidos_no_total_de_aulas_dadas()
        {
            await CriarCenarioBasico();

            await CriarAulaComFrequencia(inicioPrimeiroBimestre.AddDays(10), PROFESSOR_1, 2);
            await CriarAulaComFrequencia(inicioSegundoBimestre.AddDays(10), PROFESSOR_1, 3);
            await CriarAulaComFrequencia(inicioSegundoBimestre.AddDays(20), PROFESSOR_2, 4);
            await CriarAulaComFrequencia(inicioPrimeiroBimestre.AddDays(20), PROFESSOR_1, 8, aulaExcluida: true);
            await CriarAulaComFrequencia(inicioPrimeiroBimestre.AddDays(30), PROFESSOR_1, 16, registroExcluido: true);

            var repositorio = ServiceProvider.GetRequiredService<IRepositorioAulaConsulta>();
            var componentes = new[] { COMPONENTE_CURRICULAR_PORTUGUES_ID_138 };

            var totalPeriodoCurto = await repositorio.ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolar(
                TURMA_CODIGO_1, componentes, 1, new[] { 1L });
            var totalPeriodoLongo = await repositorio.ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolar(
                TURMA_CODIGO_1, componentes, 1, new[] { 1L, 2L });
            var totalPeriodoLongoProfessor1 = await repositorio.ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolar(
                TURMA_CODIGO_1, componentes, 1, new[] { 1L, 2L }, PROFESSOR_1);
            var totalProfessorSemAula = await repositorio.ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolar(
                TURMA_CODIGO_1, componentes, 1, new[] { 1L, 2L }, "9999999");

            totalPeriodoCurto.ShouldBe(2);
            totalPeriodoLongo.ShouldBe(9);
            totalPeriodoLongoProfessor1.ShouldBe(5);
            totalProfessorSemAula.ShouldBe(0);
        }

        [Fact]
        public async Task Deve_retornar_zero_quando_nao_houver_aula_com_frequencia_para_os_filtros()
        {
            await CriarCenarioBasico();
            await CriarAulaComFrequencia(inicioPrimeiroBimestre.AddDays(10), PROFESSOR_1, 3);

            var repositorio = ServiceProvider.GetRequiredService<IRepositorioAulaConsulta>();

            var componenteInexistente = await repositorio.ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolar(
                TURMA_CODIGO_1, new[] { 999999L }, 1, new[] { 1L });
            var periodoSemAula = await repositorio.ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolar(
                TURMA_CODIGO_1, new[] { COMPONENTE_CURRICULAR_PORTUGUES_ID_138 }, 1, new[] { 2L });

            componenteInexistente.ShouldBe(0);
            periodoSemAula.ShouldBe(0);
        }

        [Fact]
        public async Task Deve_considerar_existente_somente_frequencia_de_aluno_ativa_no_periodo_e_componente()
        {
            await CriarCenarioBasico();
            var aulaComFrequenciaAtiva = await CriarAulaComFrequencia(
                inicioPrimeiroBimestre.AddDays(10), PROFESSOR_1, 1, criarRegistroFrequenciaAluno: true);
            await CriarRegistroFrequenciaAluno(aulaComFrequenciaAtiva, 1, excluido: true);

            var repositorio = ServiceProvider.GetRequiredService<IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta>();

            var existe = await repositorio.ExisteFrequenciaRegistradaPorTurmaComponenteCurricular(
                TURMA_CODIGO_1,
                new[] { COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString() },
                1);
            var existeOutroComponente = await repositorio.ExisteFrequenciaRegistradaPorTurmaComponenteCurricular(
                TURMA_CODIGO_1,
                new[] { "999999" },
                1);
            var existeOutroPeriodo = await repositorio.ExisteFrequenciaRegistradaPorTurmaComponenteCurricular(
                TURMA_CODIGO_1,
                new[] { COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString() },
                2);
            var existeParaProfessor = await repositorio.ExisteFrequenciaRegistradaPorTurmaComponenteCurricular(
                TURMA_CODIGO_1,
                new[] { COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString() },
                1,
                PROFESSOR_1);
            var existeParaOutroProfessor = await repositorio.ExisteFrequenciaRegistradaPorTurmaComponenteCurricular(
                TURMA_CODIGO_1,
                new[] { COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString() },
                1,
                PROFESSOR_2);

            existe.ShouldBeTrue();
            existeOutroComponente.ShouldBeFalse();
            existeOutroPeriodo.ShouldBeFalse();
            existeParaProfessor.ShouldBeTrue();
            existeParaOutroProfessor.ShouldBeFalse();
        }

        [Fact]
        public async Task Deve_ignorar_frequencia_quando_todos_os_registros_de_aluno_estiverem_excluidos()
        {
            await CriarCenarioBasico();
            var aulaId = await CriarAulaComFrequencia(inicioPrimeiroBimestre.AddDays(10), PROFESSOR_1, 1);
            await CriarRegistroFrequenciaAluno(aulaId, 1, excluido: true);
            await CriarRegistroFrequenciaAluno(aulaId, 1, excluido: true);

            var repositorio = ServiceProvider.GetRequiredService<IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta>();
            var existe = await repositorio.ExisteFrequenciaRegistradaPorTurmaComponenteCurricular(
                TURMA_CODIGO_1,
                new[] { COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString() },
                1);

            existe.ShouldBeFalse();
        }

        [Fact]
        public async Task Deve_contabilizar_aulas_nas_datas_inicial_e_final_do_periodo_escolar()
        {
            await CriarCenarioBasico();
            await CriarAulaComFrequencia(inicioPrimeiroBimestre, PROFESSOR_1, 2);
            await CriarAulaComFrequencia(fimPrimeiroBimestre, PROFESSOR_1, 3);

            var repositorio = ServiceProvider.GetRequiredService<IRepositorioAulaConsulta>();
            var total = await repositorio.ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolar(
                TURMA_CODIGO_1,
                new[] { COMPONENTE_CURRICULAR_PORTUGUES_ID_138 },
                1,
                new[] { 1L });

            total.ShouldBe(5);
        }

        [Fact]
        public async Task Deve_contabilizar_aulas_do_inicio_ao_fim_do_ano_letivo()
        {
            await CriarCenarioBasicoAnoCompleto();
            await CriarAulaComFrequencia(inicioPrimeiroBimestre, PROFESSOR_1, 2);
            await CriarAulaComFrequencia(fimQuartoBimestre, PROFESSOR_1, 3);

            var repositorio = ServiceProvider.GetRequiredService<IRepositorioAulaConsulta>();
            var total = await repositorio.ObterAulasDadasPorTurmaDisciplinaEPeriodoEscolar(
                TURMA_CODIGO_1,
                new[] { COMPONENTE_CURRICULAR_PORTUGUES_ID_138 },
                1,
                new[] { 1L, 2L, 3L, 4L });

            total.ShouldBe(5);
        }

        [Fact]
        public async Task Deve_identificar_frequencia_de_aluno_na_data_final_do_periodo_escolar()
        {
            await CriarCenarioBasico();
            await CriarAulaComFrequencia(
                fimPrimeiroBimestre,
                PROFESSOR_1,
                1,
                criarRegistroFrequenciaAluno: true);

            var repositorio = ServiceProvider.GetRequiredService<IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta>();
            var existe = await repositorio.ExisteFrequenciaRegistradaPorTurmaComponenteCurricular(
                TURMA_CODIGO_1,
                new[] { COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString() },
                1);

            existe.ShouldBeTrue();
        }

        private async Task CriarCenarioBasico()
        {
            await CriarDadosBasicosAulaSemPeriodoEscolar(
                ObterPerfilProfessor(),
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio);
            await CriarPeriodoEscolar(inicioPrimeiroBimestre, fimPrimeiroBimestre, BIMESTRE_1);
            await CriarPeriodoEscolar(inicioSegundoBimestre, fimSegundoBimestre, BIMESTRE_2);
        }

        private async Task CriarCenarioBasicoAnoCompleto()
        {
            await CriarCenarioBasico();
            await CriarPeriodoEscolar(inicioTerceiroBimestre, fimTerceiroBimestre, BIMESTRE_3);
            await CriarPeriodoEscolar(inicioQuartoBimestre, fimQuartoBimestre, BIMESTRE_4);
        }

        private async Task<long> CriarAulaComFrequencia(
            DateTime dataAula,
            string professor,
            int quantidade,
            bool aulaExcluida = false,
            bool registroExcluido = false,
            bool criarRegistroFrequenciaAluno = false)
        {
            var aulaId = await InserirNaBaseAsync(new Dominio.Aula
            {
                UeId = UE_CODIGO_1,
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                TurmaId = TURMA_CODIGO_1,
                TipoCalendarioId = 1,
                ProfessorRf = professor,
                Quantidade = quantidade,
                DataAula = dataAula,
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                TipoAula = TipoAula.Normal,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Excluido = aulaExcluida
            });

            await CriarRegistroFrequencia(aulaId, registroExcluido);
            if (criarRegistroFrequenciaAluno)
                await CriarRegistroFrequenciaAluno(aulaId, 1);

            return aulaId;
        }

        private async Task<long> CriarRegistroFrequencia(long aulaId, bool excluido = false)
        {
            return await InserirNaBaseAsync(new RegistroFrequencia
            {
                AulaId = aulaId,
                Excluido = excluido,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarRegistroFrequenciaAluno(long aulaId, long registroFrequenciaId, bool excluido = false)
        {
            await InserirNaBase(new RegistroFrequenciaAluno
            {
                AulaId = aulaId,
                RegistroFrequenciaId = registroFrequenciaId,
                CodigoAluno = ALUNO_CODIGO_1,
                NumeroAula = NUMERO_AULA_1,
                Valor = (int)TipoFrequencia.C,
                Excluido = excluido,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }
}
