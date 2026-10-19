using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.Fechamento.NotaFechamentoFinal.ServicosFakes;
using SME.SGP.TesteIntegracao.NotaFechamento.ServicosFakes;
using SME.SGP.TesteIntegracao.NotaFechamentoFinal.Base;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes.Query;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.NotaFechamentoFinal
{
    /// <summary>
    /// A consulta de frequência do fechamento final passou a ser feita em lote, fora do foreach de
    /// alunos, e a somar apenas registros sem exclusão lógica. Estes testes cobrem o valor devolvido
    /// por aluno, o descarte do registro excluído e o gate de frequência registrada no componente.
    /// </summary>
    public class Ao_obter_frequencia_do_fechamento_final : NotaFechamentoTesteBase
    {
        private const int REGISTRO_FREQUENCIA_ID = 1;
        private const int TOTAL_AULAS_20 = 20;
        private const int VALOR_FREQUENCIA_2 = 2;

        public Ao_obter_frequencia_do_fechamento_final(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<PodePersistirTurmaDisciplinaQuery, bool>), typeof(PodePersistirTurmaDisciplinaQueryHandlerFakeRetornaTrue), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaEAnoLetivoQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosPorTurmaEAnoLetivoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterDadosTurmaEolPorCodigoQuery, DadosTurmaEolDto>), typeof(ObterDadosTurmaEolPorCodigoQueryHandlerFakeRegular), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterValorParametroSistemaTipoEAnoQuery, string>), typeof(ObterValorParametroSistemaTipoEAnoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesRegenciaPorAnoEolQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesRegenciaPorAnoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterPeriodoFechamentoPorCalendarioIdEBimestreQuery, PeriodoFechamentoBimestre>), typeof(ObterPeriodoFechamentoPorCalendarioIdEBimestreQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Deve_retornar_a_frequencia_correspondente_a_cada_aluno()
        {
            await CriarDadosBaseFechamentoFinal();
            await CriarFrequenciaRegistradaNoComponente();

            await CriarFrequencia(CODIGO_ALUNO_1, totalAusencias: 4, totalCompensacoes: 0);
            await CriarFrequencia(CODIGO_ALUNO_2, totalAusencias: 5, totalCompensacoes: 2);

            var retorno = await ExecutarTeste();

            var aluno1 = ObterAluno(retorno, CODIGO_ALUNO_1);
            aluno1.Frequencia.ShouldBe(Dominio.FrequenciaAluno.FormatarPercentual(80));
            aluno1.TotalFaltas.ShouldBe(4);
            aluno1.TotalAusenciasCompensadas.ShouldBe(0);

            var aluno2 = ObterAluno(retorno, CODIGO_ALUNO_2);
            aluno2.Frequencia.ShouldBe(Dominio.FrequenciaAluno.FormatarPercentual(85));
            aluno2.TotalFaltas.ShouldBe(5);
            aluno2.TotalAusenciasCompensadas.ShouldBe(2);
        }

        [Fact]
        public async Task Nao_deve_somar_registro_de_frequencia_excluido()
        {
            await CriarDadosBaseFechamentoFinal();
            await CriarFrequenciaRegistradaNoComponente();

            // O fechamento final SOMA todas as linhas do aluno, em vez de escolher uma. Se o registro
            // excluído entrasse na soma o total viraria 40 aulas e 14 ausências (65%), e não 20 e 4 (80%).
            await CriarFrequencia(CODIGO_ALUNO_1, totalAusencias: 4, totalCompensacoes: 0);
            await CriarFrequenciaExcluida(CODIGO_ALUNO_1, totalAusencias: 10, totalCompensacoes: 0, diasDeslocamento: 1);

            // O aluno 2 possui apenas registro excluído: deve ser tratado como sem frequência.
            await CriarFrequenciaExcluida(CODIGO_ALUNO_2, totalAusencias: 2, totalCompensacoes: 0);

            var retorno = await ExecutarTeste();

            var aluno1 = ObterAluno(retorno, CODIGO_ALUNO_1);
            aluno1.Frequencia.ShouldBe(Dominio.FrequenciaAluno.FormatarPercentual(80));
            aluno1.TotalFaltas.ShouldBe(4);
            aluno1.TotalAusenciasCompensadas.ShouldBe(0);

            var aluno2 = ObterAluno(retorno, CODIGO_ALUNO_2);
            aluno2.Frequencia.ShouldBeEmpty();
            aluno2.TotalFaltas.ShouldBe(0);
            aluno2.TotalAusenciasCompensadas.ShouldBe(0);
        }

        /// <summary>
        /// A frequência só é exibida quando existe registro de frequência lançado para o componente
        /// (aula + registro_frequencia + registro_frequencia_aluno). Sem isso o percentual volta nulo,
        /// mesmo havendo linha em frequencia_aluno — mas faltas e compensações continuam sendo devolvidas.
        /// </summary>
        [Fact]
        public async Task Nao_deve_retornar_percentual_quando_componente_nao_tem_frequencia_registrada()
        {
            await CriarDadosBaseFechamentoFinal();

            await CriarFrequencia(CODIGO_ALUNO_1, totalAusencias: 4, totalCompensacoes: 0);

            var retorno = await ExecutarTeste();

            var aluno1 = ObterAluno(retorno, CODIGO_ALUNO_1);
            aluno1.Frequencia.ShouldBeNull();
            aluno1.TotalFaltas.ShouldBe(4);
            aluno1.TotalAusenciasCompensadas.ShouldBe(0);
        }

        private async Task<FechamentoFinalConsultaRetornoDto> ExecutarTeste()
        {
            var consulta = ServiceProvider.GetService<IConsultasFechamentoFinal>();

            return await consulta.ObterFechamentos(new FechamentoFinalConsultaFiltroDto
            {
                DisciplinaCodigo = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                TurmaCodigo = TURMA_CODIGO_1
            });
        }

        private static FechamentoFinalConsultaRetornoAlunoDto ObterAluno(FechamentoFinalConsultaRetornoDto retorno, string codigoAluno)
        {
            retorno.ShouldNotBeNull();
            retorno.Alunos.ShouldNotBeNull();

            var aluno = retorno.Alunos.FirstOrDefault(a => a.Codigo == codigoAluno);
            aluno.ShouldNotBeNull();

            return aluno;
        }

        private async Task CriarDadosBaseFechamentoFinal()
        {
            var filtroNotaFechamento = ObterFiltroNotas(
                ObterPerfilProfessor(),
                ANO_7,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                TipoNota.Nota,
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                false);

            await CriarDadosBase(filtroNotaFechamento);
            await CriarFechamentoTodosBimestres();
        }

        /// <summary>
        /// Sem o fechamento do último bimestre a consulta lança exceção de negócio antes de chegar na
        /// listagem de alunos. O fechamento_aluno é exigido pelo inner join da consulta.
        /// </summary>
        private async Task CriarFechamentoTodosBimestres()
        {
            await CriarFechamentoTurmaDisciplina(PERIODO_ESCOLAR_CODIGO_1, FECHAMENTO_TURMA_ID_1);
            await CriarFechamentoTurmaDisciplina(PERIODO_ESCOLAR_CODIGO_2, FECHAMENTO_TURMA_ID_2);
            await CriarFechamentoTurmaDisciplina(PERIODO_ESCOLAR_CODIGO_3, FECHAMENTO_TURMA_ID_3);
            await CriarFechamentoTurmaDisciplina(PERIODO_ESCOLAR_CODIGO_4, FECHAMENTO_TURMA_ID_4);

            foreach (var fechamentoTurmaDisciplinaId in new[] { FECHAMENTO_TURMA_DISCIPLINA_ID_1, FECHAMENTO_TURMA_DISCIPLINA_ID_2, FECHAMENTO_TURMA_DISCIPLINA_ID_3, FECHAMENTO_TURMA_DISCIPLINA_ID_4 })
            {
                await CriarFechamentoAluno(fechamentoTurmaDisciplinaId, CODIGO_ALUNO_1);
                await CriarFechamentoAluno(fechamentoTurmaDisciplinaId, CODIGO_ALUNO_2);
            }
        }

        private async Task CriarFechamentoTurmaDisciplina(long periodoEscolarId, long fechamentoTurmaId)
        {
            await InserirNaBase(new FechamentoTurma
            {
                TurmaId = TURMA_ID_1,
                PeriodoEscolarId = periodoEscolarId,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoTurmaDisciplina
            {
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                FechamentoTurmaId = fechamentoTurmaId,
                Situacao = SituacaoFechamento.ProcessadoComSucesso,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarFechamentoAluno(long fechamentoTurmaDisciplinaId, string codigoAluno)
        {
            await InserirNaBase(new FechamentoAluno
            {
                AlunoCodigo = codigoAluno,
                FechamentoTurmaDisciplinaId = fechamentoTurmaDisciplinaId,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarFrequenciaRegistradaNoComponente()
        {
            await CriarAula(
                DATA_INICIO_BIMESTRE_4,
                RecorrenciaAula.AulaUnica,
                TipoAula.Normal,
                USUARIO_PROFESSOR_CODIGO_RF_2222222,
                TURMA_CODIGO_1,
                UE_CODIGO_1,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                TIPO_CALENDARIO_1);

            await InserirNaBase(new RegistroFrequencia
            {
                AulaId = AULA_ID,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            foreach (var codigoAluno in new[] { CODIGO_ALUNO_1, CODIGO_ALUNO_2 })
            {
                await InserirNaBase(new RegistroFrequenciaAluno
                {
                    CodigoAluno = codigoAluno,
                    RegistroFrequenciaId = REGISTRO_FREQUENCIA_ID,
                    Valor = VALOR_FREQUENCIA_2,
                    NumeroAula = QUANTIDADE_AULA_4,
                    AulaId = AULA_ID,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });
            }
        }

        /// <summary>
        /// A constraint frequencia_aluno_un impede dois registros com o mesmo
        /// (codigo_aluno, tipo, disciplina_id, periodo_inicio, periodo_fim, turma_id), por isso o
        /// deslocamento de dias para representar a duplicidade que ocorre na base real.
        /// </summary>
        private async Task CriarFrequencia(string codigoAluno, int totalAusencias, int totalCompensacoes, int diasDeslocamento = 0)
        {
            await InserirNaBase(new Dominio.FrequenciaAluno
            {
                CodigoAluno = codigoAluno,
                Tipo = TipoFrequenciaAluno.PorDisciplina,
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                PeriodoEscolarId = PERIODO_ESCOLAR_CODIGO_1,
                Bimestre = BIMESTRE_1,
                PeriodoInicio = DATA_01_02_INICIO_BIMESTRE_1.AddDays(diasDeslocamento),
                PeriodoFim = DATA_25_04_FIM_BIMESTRE_1,
                TotalAulas = TOTAL_AULAS_20,
                TotalAusencias = totalAusencias,
                TotalCompensacoes = totalCompensacoes,
                TurmaId = TURMA_CODIGO_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        /// <summary>
        /// A entidade FrequenciaAluno não expõe a coluna excluido, por isso o registro
        /// com exclusão lógica precisa ser inserido diretamente na tabela.
        /// </summary>
        private async Task CriarFrequenciaExcluida(string codigoAluno, int totalAusencias, int totalCompensacoes, int diasDeslocamento = 0)
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
                $"'{DATA_01_02_INICIO_BIMESTRE_1.AddDays(diasDeslocamento):yyyy-MM-dd}'",
                $"'{DATA_25_04_FIM_BIMESTRE_1:yyyy-MM-dd}'",
                $"{BIMESTRE_1}",
                $"{TOTAL_AULAS_20}",
                $"{totalAusencias}",
                $"{totalCompensacoes}",
                $"'{TURMA_CODIGO_1}'",
                $"{PERIODO_ESCOLAR_CODIGO_1}",
                $"'{DateTimeExtension.HorarioBrasilia():yyyy-MM-dd}'",
                $"'{SISTEMA_NOME}'",
                $"'{SISTEMA_CODIGO_RF}'",
                "true"
            };

            await InserirNaBase("frequencia_aluno", campos, valores);
        }
    }
}
