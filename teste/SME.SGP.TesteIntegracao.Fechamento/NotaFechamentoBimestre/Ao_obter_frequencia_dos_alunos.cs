using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.NotaFechamentoBimestre.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.NotaFechamentoBimestre
{
    public class Ao_obter_frequencia_dos_alunos : NotaFechamentoBimestreTesteBase
    {
        private const int TOTAL_AULAS_20 = 20;
        private const long FECHAMENTO_TURMA_ID_1 = 1;
        private const long FECHAMENTO_TURMA_DISCIPLINA_ID_1 = 1;

        public Ao_obter_frequencia_dos_alunos(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaEAnoLetivoQuery, IEnumerable<AlunoPorTurmaResposta>>),
                typeof(ObterAlunosPorTurmaEAnoLetivoQueryHandlerFakeValidarAlunos), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery, IEnumerable<ComponenteCurricularEol>>),
                typeof(ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQueryHandlerFakePortugues), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQuery, IEnumerable<ComponenteCurricularEol>>),
                typeof(ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQueryHandlerFakePortugues), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterDadosTurmaEolPorCodigoQuery, DadosTurmaEolDto>),
                typeof(ObterDadosTurmaEolPorCodigoQueryHandlerFakeRegular), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTodosAlunosNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>),
                typeof(ObterTodosAlunosNaTurmaQueryHandlerFake), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterMatriculasAlunoNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>),
                typeof(ObterMatriculasAlunoNaTurmaQueryHandlerFakeAlunoCodigo1), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Deve_retornar_a_frequencia_correspondente_a_cada_aluno()
        {
            var periodoEscolar = await CriarDadosBaseFrequencia();

            await CriarFrequenciaComponente(periodoEscolar, CODIGO_ALUNO_1, totalAusencias: 4, totalCompensacoes: 0);
            await CriarFrequenciaComponente(periodoEscolar, CODIGO_ALUNO_2, totalAusencias: 5, totalCompensacoes: 2);
            await CriarFrequenciaComponente(periodoEscolar, CODIGO_ALUNO_3, totalAusencias: 10, totalCompensacoes: 0);

            var retorno = await ExecutarTeste();

            ObterFrequenciaDoAluno(retorno, CODIGO_ALUNO_1).ShouldBe(Dominio.FrequenciaAluno.FormatarPercentual(80));
            ObterFrequenciaDoAluno(retorno, CODIGO_ALUNO_2).ShouldBe(Dominio.FrequenciaAluno.FormatarPercentual(85));
            ObterFrequenciaDoAluno(retorno, CODIGO_ALUNO_3).ShouldBe(Dominio.FrequenciaAluno.FormatarPercentual(50));
        }

        [Fact]
        public async Task Nao_deve_retornar_frequencia_para_aluno_sem_registro_no_componente()
        {
            var periodoEscolar = await CriarDadosBaseFrequencia();

            await CriarFrequenciaComponente(periodoEscolar, CODIGO_ALUNO_1, totalAusencias: 4, totalCompensacoes: 0);

            var retorno = await ExecutarTeste();

            ObterFrequenciaDoAluno(retorno, CODIGO_ALUNO_1).ShouldBe(Dominio.FrequenciaAluno.FormatarPercentual(80));
            ObterFrequenciaDoAluno(retorno, CODIGO_ALUNO_2).ShouldBeNull();
        }

        [Fact]
        public async Task Nao_deve_considerar_registro_de_frequencia_excluido()
        {
            var periodoEscolar = await CriarDadosBaseFrequencia();

            // O registro excluído é o mais recente: sem o filtro lógico, seria ele a vencer o desempate.
            await CriarFrequenciaComponente(periodoEscolar, CODIGO_ALUNO_1, totalAusencias: 4, totalCompensacoes: 0);
            await CriarFrequenciaComponenteExcluida(periodoEscolar, CODIGO_ALUNO_1, totalAusencias: 0, totalCompensacoes: 0, diasDeslocamento: 1);

            await CriarFrequenciaComponenteExcluida(periodoEscolar, CODIGO_ALUNO_2, totalAusencias: 2, totalCompensacoes: 0);

            var retorno = await ExecutarTeste();

            ObterFrequenciaDoAluno(retorno, CODIGO_ALUNO_1).ShouldBe(Dominio.FrequenciaAluno.FormatarPercentual(80));
            ObterFrequenciaDoAluno(retorno, CODIGO_ALUNO_2).ShouldBeNull();
        }

        [Fact]
        public async Task Deve_considerar_o_registro_mais_recente_quando_houver_duplicidade()
        {
            var periodoEscolar = await CriarDadosBaseFrequencia();

            await CriarFrequenciaComponente(periodoEscolar, CODIGO_ALUNO_1, totalAusencias: 10, totalCompensacoes: 0);
            await CriarFrequenciaComponente(periodoEscolar, CODIGO_ALUNO_1, totalAusencias: 4, totalCompensacoes: 0, diasDeslocamento: 1);

            var retorno = await ExecutarTeste();

            ObterFrequenciaDoAluno(retorno, CODIGO_ALUNO_1).ShouldBe(Dominio.FrequenciaAluno.FormatarPercentual(80));
        }

        [Fact]
        public async Task Deve_retornar_faltas_e_compensacoes_de_cada_aluno_nas_notas_do_fechamento()
        {
            var periodoEscolar = await CriarDadosBaseFrequencia();

            await CriarFrequenciaComponente(periodoEscolar, CODIGO_ALUNO_1, totalAusencias: 4, totalCompensacoes: 0);
            await CriarFrequenciaComponente(periodoEscolar, CODIGO_ALUNO_2, totalAusencias: 5, totalCompensacoes: 2);

            // O aluno 3 possui apenas registro excluído: deve ser tratado como sem frequência.
            await CriarFrequenciaComponenteExcluida(periodoEscolar, CODIGO_ALUNO_3, totalAusencias: 2, totalCompensacoes: 0);

            await CriarFechamentoTurmaDisciplina(periodoEscolar);

            var consultas = ServiceProvider.GetService<IConsultasFechamentoTurmaDisciplina>();
            var retorno = await consultas.ObterNotasFechamentoTurmaDisciplina(TURMA_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, BIMESTRE_1, SEMESTRE_0);

            var aluno1 = ObterAlunoDoFechamento(retorno, CODIGO_ALUNO_1);
            aluno1.PercentualFrequencia.ShouldBe(Dominio.FrequenciaAluno.FormatarPercentual(80));
            aluno1.QuantidadeFaltas.ShouldBe(4);
            aluno1.QuantidadeCompensacoes.ShouldBe(0);

            var aluno2 = ObterAlunoDoFechamento(retorno, CODIGO_ALUNO_2);
            aluno2.PercentualFrequencia.ShouldBe(Dominio.FrequenciaAluno.FormatarPercentual(85));
            aluno2.QuantidadeFaltas.ShouldBe(5);
            aluno2.QuantidadeCompensacoes.ShouldBe(2);

            var aluno3 = ObterAlunoDoFechamento(retorno, CODIGO_ALUNO_3);
            aluno3.PercentualFrequencia.ShouldBeEmpty();
            aluno3.QuantidadeFaltas.ShouldBe(0);
            aluno3.QuantidadeCompensacoes.ShouldBe(0);
        }

        [Fact]
        public async Task Nao_deve_considerar_registro_excluido_mais_recente_nas_notas_do_fechamento()
        {
            var periodoEscolar = await CriarDadosBaseFrequencia();

            // O aluno tem registro válido E registro excluído. O excluído é o mais recente: sem o
            // filtro lógico ele venceria o row_number da consulta e o aluno apareceria com 100%
            // de frequência e nenhuma falta.
            await CriarFrequenciaComponente(periodoEscolar, CODIGO_ALUNO_1, totalAusencias: 4, totalCompensacoes: 0);
            await CriarFrequenciaComponenteExcluida(periodoEscolar, CODIGO_ALUNO_1, totalAusencias: 0, totalCompensacoes: 0, diasDeslocamento: 1);

            await CriarFechamentoTurmaDisciplina(periodoEscolar);

            var consultas = ServiceProvider.GetService<IConsultasFechamentoTurmaDisciplina>();
            var retorno = await consultas.ObterNotasFechamentoTurmaDisciplina(TURMA_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, BIMESTRE_1, SEMESTRE_0);

            var aluno1 = ObterAlunoDoFechamento(retorno, CODIGO_ALUNO_1);
            aluno1.PercentualFrequencia.ShouldBe(Dominio.FrequenciaAluno.FormatarPercentual(80));
            aluno1.QuantidadeFaltas.ShouldBe(4);
            aluno1.QuantidadeCompensacoes.ShouldBe(0);
        }

        [Fact]
        public async Task Deve_somar_a_frequencia_de_todos_os_bimestres_no_fechamento_final()
        {
            var primeiroBimestre = await CriarDadosBaseFrequencia();
            var segundoBimestre = ObterPeriodoEscolar(BIMESTRE_2);

            await CriarFrequenciaComponente(primeiroBimestre, CODIGO_ALUNO_1, totalAusencias: 4, totalCompensacoes: 0);
            await CriarFrequenciaComponente(segundoBimestre, CODIGO_ALUNO_1, totalAusencias: 6, totalCompensacoes: 0);

            var retorno = await ExecutarTesteFechamentoFinal();

            // 40 aulas e 10 faltas somadas nos dois bimestres.
            ObterFrequenciaDoAluno(retorno, CODIGO_ALUNO_1).ShouldBe(Dominio.FrequenciaAluno.FormatarPercentual(75));
        }

        [Fact]
        public async Task Deve_retornar_percentual_zerado_para_aluno_sem_frequencia_no_fechamento_final()
        {
            var primeiroBimestre = await CriarDadosBaseFrequencia();

            await CriarFrequenciaComponente(primeiroBimestre, CODIGO_ALUNO_1, totalAusencias: 4, totalCompensacoes: 0);

            var retorno = await ExecutarTesteFechamentoFinal();

            ObterFrequenciaDoAluno(retorno, CODIGO_ALUNO_1).ShouldBe(Dominio.FrequenciaAluno.FormatarPercentual(80));
            ObterFrequenciaDoAluno(retorno, CODIGO_ALUNO_2).ShouldBe(Dominio.FrequenciaAluno.FormatarPercentual(0));
        }

        [Fact]
        public async Task Nao_deve_considerar_registro_de_frequencia_excluido_no_fechamento_final()
        {
            var primeiroBimestre = await CriarDadosBaseFrequencia();

            await CriarFrequenciaComponenteExcluida(primeiroBimestre, CODIGO_ALUNO_3, totalAusencias: 2, totalCompensacoes: 0);

            var retorno = await ExecutarTesteFechamentoFinal();

            ObterFrequenciaDoAluno(retorno, CODIGO_ALUNO_3).ShouldBe(Dominio.FrequenciaAluno.FormatarPercentual(0));
        }

        [Fact]
        public async Task Nao_deve_somar_registro_excluido_na_frequencia_do_fechamento_final()
        {
            var primeiroBimestre = await CriarDadosBaseFrequencia();

            // O fechamento final SOMA todas as linhas do aluno, em vez de escolher uma. Se o registro
            // excluído entrasse na soma o total viraria 40 aulas e 14 ausências (65%), e não 20 e 4 (80%).
            await CriarFrequenciaComponente(primeiroBimestre, CODIGO_ALUNO_1, totalAusencias: 4, totalCompensacoes: 0);
            await CriarFrequenciaComponenteExcluida(primeiroBimestre, CODIGO_ALUNO_1, totalAusencias: 10, totalCompensacoes: 0, diasDeslocamento: 1);

            var retorno = await ExecutarTesteFechamentoFinal();

            ObterFrequenciaDoAluno(retorno, CODIGO_ALUNO_1).ShouldBe(Dominio.FrequenciaAluno.FormatarPercentual(80));
        }

        private static NotaConceitoAlunoBimestreDto ObterAlunoDoFechamento(FechamentoTurmaDisciplinaBimestreDto retorno, string codigoAluno)
        {
            var aluno = retorno.Alunos.FirstOrDefault(a => a.CodigoAluno == codigoAluno);

            aluno.ShouldNotBeNull($"Aluno {codigoAluno} não retornado. Alunos: {string.Join(", ", retorno.Alunos.Select(a => a.CodigoAluno))}");

            return aluno;
        }

        private async Task CriarFechamentoTurmaDisciplina(PeriodoEscolar periodoEscolar)
        {
            await InserirNaBase(new FechamentoTurma
            {
                TurmaId = TURMA_ID_1,
                PeriodoEscolarId = periodoEscolar.Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoTurmaDisciplina
            {
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                FechamentoTurmaId = FECHAMENTO_TURMA_ID_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            // A consulta de fechamento faz inner join em fechamento_aluno: sem esses registros
            // nenhum fechamento é localizado e a listagem volta vazia.
            foreach (var codigoAluno in new[] { CODIGO_ALUNO_1, CODIGO_ALUNO_2, CODIGO_ALUNO_3 })
            {
                await InserirNaBase(new FechamentoAluno
                {
                    FechamentoTurmaDisciplinaId = FECHAMENTO_TURMA_DISCIPLINA_ID_1,
                    AlunoCodigo = codigoAluno,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });
            }
        }

        private static string ObterFrequenciaDoAluno(FechamentoNotaConceitoTurmaDto retorno, string codigoAluno)
        {
            var aluno = retorno.Alunos.FirstOrDefault(a => a.CodigoAluno == codigoAluno);

            aluno.ShouldNotBeNull();

            return aluno.Frequencia;
        }

        private async Task<PeriodoEscolar> CriarDadosBaseFrequencia()
        {
            var filtroNotaFechamento = ObterFiltroFechamentoNotaFrequencia();

            await InserirPeriodoEscolarCustomizado();
            await CriarDadosBase(filtroNotaFechamento);
            await CriarTipoAvaliacao(TipoAvaliacaoCodigo.AvaliacaoBimestral, AVALIACAO_NOME_1);

            return ObterPeriodoEscolar(BIMESTRE_1);
        }

        private async Task<FechamentoNotaConceitoTurmaDto> ExecutarTeste()
        {
            var useCase = ServiceProvider.GetService<IListarFechamentoTurmaBimestreUseCase>();

            return await useCase.Executar(TURMA_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, BIMESTRE_1, SEMESTRE_0);
        }

        /// <summary>
        /// O fechamento final exige o fechamento do último bimestre, senão
        /// VerificaSePodeFazerFechamentoFinal lança exceção de negócio.
        /// </summary>
        private async Task<FechamentoNotaConceitoTurmaDto> ExecutarTesteFechamentoFinal()
        {
            await CriarFechamentoTurmaDisciplina(ObterPeriodoEscolar(BIMESTRE_4));

            var useCase = ServiceProvider.GetService<IListarFechamentoTurmaBimestreUseCase>();

            return await useCase.Executar(TURMA_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, BIMESTRE_FINAL, SEMESTRE_0);
        }

        private PeriodoEscolar ObterPeriodoEscolar(int bimestre)
            => ObterTodos<PeriodoEscolar>().FirstOrDefault(c => c.Bimestre == bimestre);

        /// <summary>
        /// A constraint frequencia_aluno_un impede dois registros com o mesmo
        /// (codigo_aluno, tipo, disciplina_id, periodo_inicio, periodo_fim, turma_id). Na base real a
        /// duplicidade aparece com periodo_inicio/periodo_fim distintos para o mesmo periodo_escolar_id
        /// — é o agrupamento usado por RemoverFrequenciasDuplicadas —, por isso o deslocamento de dias.
        /// </summary>
        private async Task CriarFrequenciaComponente(PeriodoEscolar periodoEscolar, string codigoAluno, int totalAusencias, int totalCompensacoes, int diasDeslocamento = 0)
        {
            await InserirNaBase(new Dominio.FrequenciaAluno
            {
                CodigoAluno = codigoAluno,
                Tipo = TipoFrequenciaAluno.PorDisciplina,
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                PeriodoInicio = periodoEscolar.PeriodoInicio.AddDays(diasDeslocamento),
                PeriodoFim = periodoEscolar.PeriodoFim,
                Bimestre = periodoEscolar.Bimestre,
                TotalAulas = TOTAL_AULAS_20,
                TotalAusencias = totalAusencias,
                TotalCompensacoes = totalCompensacoes,
                TurmaId = TURMA_CODIGO_1,
                PeriodoEscolarId = periodoEscolar.Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        /// <summary>
        /// A entidade FrequenciaAluno não expõe a coluna excluido, por isso o registro
        /// com exclusão lógica precisa ser inserido diretamente na tabela.
        /// </summary>
        private async Task CriarFrequenciaComponenteExcluida(PeriodoEscolar periodoEscolar, string codigoAluno, int totalAusencias, int totalCompensacoes, int diasDeslocamento = 0)
        {
            var campos = new[]
            {
                "codigo_aluno", "tipo", "disciplina_id", "periodo_inicio", "periodo_fim", "bimestre",
                "total_aulas", "total_ausencias", "total_compensacoes", "turma_id", "periodo_escolar_id",
                "criado_em", "criado_por", "criado_rf", "excluido"
            };

            var valores = new[]
            {
                $"'{codigoAluno}'",
                $"{(int)TipoFrequenciaAluno.PorDisciplina}",
                $"'{COMPONENTE_CURRICULAR_PORTUGUES_ID_138}'",
                $"'{periodoEscolar.PeriodoInicio.AddDays(diasDeslocamento):yyyy-MM-dd}'",
                $"'{periodoEscolar.PeriodoFim:yyyy-MM-dd}'",
                $"{periodoEscolar.Bimestre}",
                $"{TOTAL_AULAS_20}",
                $"{totalAusencias}",
                $"{totalCompensacoes}",
                $"'{TURMA_CODIGO_1}'",
                $"{periodoEscolar.Id}",
                $"'{DateTimeExtension.HorarioBrasilia():yyyy-MM-dd}'",
                $"'{SISTEMA_NOME}'",
                $"'{SISTEMA_CODIGO_RF}'",
                "true"
            };

            await InserirNaBase("frequencia_aluno", campos, valores);
        }

        private FiltroFechamentoNotaDto ObterFiltroFechamentoNotaFrequencia()
        {
            return new FiltroFechamentoNotaDto
            {
                Perfil = ObterPerfilProfessor(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                ConsiderarAnoAnterior = false,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = ANO_7,
                TipoFrequenciaAluno = TipoFrequenciaAluno.PorDisciplina,
                ProfessorRf = USUARIO_PROFESSOR_LOGIN_2222222,
                ComponenteCurricular = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                CriarPeriodoEscolar = false,
                CriarPeriodoEscolarCustomizado = false
            };
        }

        private async Task InserirPeriodoEscolarCustomizado()
        {
            var dataReferencia = DateTimeExtension.HorarioBrasilia().Date;

            await CriarPeriodoEscolar(dataReferencia.AddDays(-45), dataReferencia.AddDays(+30), BIMESTRE_1);
            await CriarPeriodoEscolar(dataReferencia.AddDays(40), dataReferencia.AddDays(115), BIMESTRE_2);
            await CriarPeriodoEscolar(dataReferencia.AddDays(125), dataReferencia.AddDays(200), BIMESTRE_3);
            await CriarPeriodoEscolar(dataReferencia.AddDays(210), dataReferencia.AddDays(285), BIMESTRE_4);
        }
    }
}
