using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.CompensacaoDeAusencia.Base;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.CompensacaoDeAusencia
{
    public class Ao_lancar_compensacao_ausencia_bimestre_encerrado : CompensacaoDeAusenciaTesteBase
    {
        public Ao_lancar_compensacao_ausencia_bimestre_encerrado(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Deve_bloquear_lancar_compensacao_ausencia_ano_anterior_sem_reabertura_periodo()
        {
            await CriarPeriodoEscolar();
            
            var periodosEscolares = ObterTodos<PeriodoEscolar>();

            var dataInicioPrimeiroBimestre =
                periodosEscolares.FirstOrDefault(c => c.Bimestre == BIMESTRE_1)!.PeriodoInicio;

            var compensacaoDeAusencia = await ObterCompensacaoDeAusencia(ObterPerfilProfessor(),
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                ANO_7,
                dataInicioPrimeiroBimestre,
                22,
                false);
            
            await CriarDadosBase(compensacaoDeAusencia);
            await CriarFrequenciasAlunos(BIMESTRE_1, compensacaoDeAusencia.ComponenteCurricular);
            await CriarRegistroFrequencia();            

            var compensacaoAusenciaDosAlunos = await LancarCompensacaoAusenciasAlunos(BIMESTRE_1, compensacaoDeAusencia.ComponenteCurricular);
            
            var comando = ServiceProvider.GetService<IComandosCompensacaoAusencia>();
            
            comando.ShouldNotBeNull();

            async Task doExecutarInserir() { await comando.Inserir(compensacaoAusenciaDosAlunos); }
            await Should.ThrowAsync<NegocioException>(() => doExecutarInserir());    
        }
        
        [Fact]
        public async Task Deve_bloquear_lancar_compensacao_ausencia_componente_que_nao_lanca_frequencia()
        {
            await CriarPeriodoEscolar();
            
            var periodosEscolares = ObterTodos<PeriodoEscolar>();

            var dataInicioPrimeiroBimestre =
                periodosEscolares.FirstOrDefault(c => c.Bimestre == BIMESTRE_1)!.PeriodoInicio;

            var compensacaoDeAusencia = await ObterCompensacaoDeAusencia(ObterPerfilProfessor(),
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                COMPONENTE_CURRICULAR_AULA_COMPARTILHADA.ToString(),
                ANO_7,
                dataInicioPrimeiroBimestre,
                22,
                false);
            
            await CriarDadosBase(compensacaoDeAusencia);
            await CriarFrequenciasAlunos(BIMESTRE_1, compensacaoDeAusencia.ComponenteCurricular);
            await CriarRegistroFrequencia();            

            var compensacaoAusenciaDosAlunos = await LancarCompensacaoAusenciasAlunos(BIMESTRE_1, compensacaoDeAusencia.ComponenteCurricular);
            
            var comando = ServiceProvider.GetService<IComandosCompensacaoAusencia>();
            
            comando.ShouldNotBeNull();

            async Task doExecutarInserir() { await comando.Inserir(compensacaoAusenciaDosAlunos); }
            await Should.ThrowAsync<NegocioException>(() => doExecutarInserir());
        }
        
        private static async Task<CompensacaoAusenciaDto> LancarCompensacaoAusenciasAlunos(int bimestre, string disciplinaId)
        {
            //-> Item1 = Código do aluno
            //   Item2 = Quantidade de aulas
            var compensacaoAusenciasAlunos = new List<Tuple<string, int>>
            {
                new(CODIGO_ALUNO_1, QUANTIDADE_AULA),
                new(CODIGO_ALUNO_2, QUANTIDADE_AULA_2),
                new(CODIGO_ALUNO_3, QUANTIDADE_AULA_3),
                new(CODIGO_ALUNO_4, QUANTIDADE_AULA_4)
            };

            List<CompensacaoAusenciaAlunoDto> alunos = new();

            foreach (var compensacaoAusenciaAluno in compensacaoAusenciasAlunos)
            {
                alunos.Add(new CompensacaoAusenciaAlunoDto
                {
                    Id = compensacaoAusenciaAluno.Item1,
                    QtdFaltasCompensadas = compensacaoAusenciaAluno.Item2
                });
            };
            
            return await Task.FromResult(new CompensacaoAusenciaDto
            {
                TurmaId = TURMA_CODIGO_1,
                Alunos = alunos,
                Bimestre = bimestre,
                Atividade = ATIVIDADE_COMPENSACAO,
                Descricao = DESCRICAO_COMPENSACAO,
                DisciplinaId = disciplinaId,
                DisciplinasRegenciaIds = null
            });
        }        

        private async Task CriarPeriodoEscolar()
        {
            await CriarPeriodoEscolar(DATA_03_01_INICIO_BIMESTRE_1, DATA_29_04_FIM_BIMESTRE_1, BIMESTRE_1);
            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_08_07_FIM_BIMESTRE_2, BIMESTRE_2);
            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_30_09_FIM_BIMESTRE_3, BIMESTRE_3);
            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4);            
        }
        
        private async Task CriarPeriodoEscolarAnoAnterior()
        {
            await CriarPeriodoEscolar(DATA_03_01_INICIO_BIMESTRE_1, DATA_29_04_FIM_BIMESTRE_1, BIMESTRE_1);
            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_08_07_FIM_BIMESTRE_2, BIMESTRE_2);
            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_30_09_FIM_BIMESTRE_3, BIMESTRE_3);
            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4);            
        }        

        private async Task CriarRegistroFrequencia()
        {
            await InserirNaBase(new RegistroFrequencia
            {
                AulaId = AULA_ID,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
            
            //-> Item1 = Código do aluno
            //   Item2 = Quantidade de aulas
            //   Item3 = Tipo da frequência
            var registrosFrequenciasAlunos = new List<Tuple<string, int, TipoFrequencia>>
            {
                new(CODIGO_ALUNO_1, QUANTIDADE_AULA_4, TipoFrequencia.F),
                new(CODIGO_ALUNO_2, QUANTIDADE_AULA_4, TipoFrequencia.C),
                new(CODIGO_ALUNO_3, QUANTIDADE_AULA_4, TipoFrequencia.R),
                new(CODIGO_ALUNO_4, QUANTIDADE_AULA_4, TipoFrequencia.F)
            };

            foreach (var registroFrequenciaAluno in registrosFrequenciasAlunos)
            {
                await CriarRegistrosFrequenciasAlunos(registroFrequenciaAluno.Item1, registroFrequenciaAluno.Item2,
                    registroFrequenciaAluno.Item3);
            }
        }

        private async Task CriarRegistrosFrequenciasAlunos(string codigoAluno, int numeroAula,
            TipoFrequencia tipoFrequencia)
        {
            await InserirNaBase(new RegistroFrequenciaAluno
            {
                CodigoAluno = codigoAluno,
                RegistroFrequenciaId = REGISTRO_FREQUENCIA_ID_1,
                Valor = (int)tipoFrequencia,
                NumeroAula = numeroAula,
                AulaId = AULA_ID,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });            
        }

        private async Task CriarFrequenciasAlunos(int bimestre, string disciplinaId)
        {
            var periodosEscolares = ObterTodos<PeriodoEscolar>();
            var periodoEscolar = periodosEscolares.FirstOrDefault(c => c.Bimestre == bimestre);
            
            if (periodoEscolar == null)
                return;

            const int totalAulas = 22;

            // -> Item1 = Código aluno
            //    Item2 = Total de ausências
            //    Item3 = Total de compensações
            //    Item4 = Total de presenças
            //    Item5 = Total remotos
            var frequenciasAlunos = new List<Tuple<string, int, int, int, int>>
            {
                new(CODIGO_ALUNO_1, TOTAL_AUSENCIAS_1, TOTAL_COMPENSACOES_1, totalAulas, TOTAL_REMOTOS_0),
                new(CODIGO_ALUNO_2, TOTAL_AUSENCIAS_3, TOTAL_COMPENSACOES_3, totalAulas, TOTAL_REMOTOS_0),
                new(CODIGO_ALUNO_3, TOTAL_AUSENCIAS_7, TOTAL_COMPENSACOES_7, totalAulas, TOTAL_REMOTOS_0),
                new(CODIGO_ALUNO_4, TOTAL_AUSENCIAS_8, TOTAL_COMPENSACOES_8, totalAulas, TOTAL_REMOTOS_0)
            };

            foreach (var frequenciaAluno in frequenciasAlunos)
            {
                await InserirNaBase(new Dominio.FrequenciaAluno
                {
                    PeriodoInicio = periodoEscolar.PeriodoInicio,
                    PeriodoFim = periodoEscolar.PeriodoFim,
                    Bimestre = bimestre,
                    TotalAulas = totalAulas,
                    TotalAusencias = frequenciaAluno.Item2,
                    TotalCompensacoes = frequenciaAluno.Item3,
                    PeriodoEscolarId = periodoEscolar.Id,
                    TotalPresencas = frequenciaAluno.Item4,
                    TotalRemotos = frequenciaAluno.Item5,
                    DisciplinaId = disciplinaId,
                    CodigoAluno = frequenciaAluno.Item1,
                    TurmaId = TURMA_CODIGO_1,
                    Tipo = TipoFrequenciaAluno.PorDisciplina,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });
            }
        }
        
        private static async Task<CompensacaoDeAusenciaDBDto> ObterCompensacaoDeAusencia(string perfil, Modalidade modalidade, 
            ModalidadeTipoCalendario modalidadeTipoCalendario, string componenteCurricular, string anoTurma, 
            DateTime dataReferencia, int quantidadeAulas, bool criarPeriodoAbertura)
        {
            return await Task.FromResult(new CompensacaoDeAusenciaDBDto
            {
                Perfil = perfil,
                Modalidade = modalidade,
                TipoCalendario = modalidadeTipoCalendario,
                Bimestre = BIMESTRE_1,
                ComponenteCurricular = componenteCurricular,
                TipoCalendarioId = TIPO_CALENDARIO_1,
                AnoTurma = anoTurma,
                DataReferencia = dataReferencia,
                QuantidadeAula = quantidadeAulas,
                CriarPeriodoEscolar = false,
                CriarPeriodoAbertura = criarPeriodoAbertura
            });
        }        
    }
}