using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;

namespace SME.SGP.TesteIntegracao.CompensacaoDeAusencia.Base
{
    public class Ao_lancar_compensacao_ausencia_bimestre_base : CompensacaoDeAusenciaTesteBase
    {
        protected const int QUANTIDADE_AULAS_22 = 22;

        public Ao_lancar_compensacao_ausencia_bimestre_base(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        
        protected static async Task<List<(string CodigoAluno, int QdadeAula)>> ObterCompensacaoAusenciasAlunos()
        {
            //-> Item1 = Código do aluno
            //   Item2 = Quantidade de aulas
            return await Task.FromResult(new List<(string CodigoAluno, int QdadeAula)>
            {
                (CODIGO_ALUNO_1, QUANTIDADE_AULA),
                (CODIGO_ALUNO_2, QUANTIDADE_AULA_2),
                (CODIGO_ALUNO_3, QUANTIDADE_AULA_3),
                (CODIGO_ALUNO_4, QUANTIDADE_AULA_4)
            });
        }        
        
        protected async Task<CompensacaoAusenciaDto> LancarCompensacaoAusenciasAlunos(CompensacaoDeAusenciaDBDto compensacaoDeAusencia)
        {
            await CriarDadosBase(compensacaoDeAusencia);
            await CriarFrequenciasAlunos(compensacaoDeAusencia.Bimestre, compensacaoDeAusencia.ComponenteCurricular);
            await CriarRegistroFrequencia();

            var compensacaoAusenciasAlunos = await ObterCompensacaoAusenciasAlunos();

            List<CompensacaoAusenciaAlunoDto> alunos = new();

            foreach (var compensacaoAusenciaAluno in compensacaoAusenciasAlunos)
            {
                alunos.Add(new CompensacaoAusenciaAlunoDto
                {
                    Id = compensacaoAusenciaAluno.CodigoAluno,
                    QtdFaltasCompensadas = compensacaoAusenciaAluno.QdadeAula
                });
            };
            
            return await Task.FromResult(new CompensacaoAusenciaDto
            {
                TurmaId = TURMA_CODIGO_1,
                Alunos = alunos,
                Bimestre = compensacaoDeAusencia.Bimestre,
                Atividade = ATIVIDADE_COMPENSACAO,
                Descricao = DESCRICAO_COMPENSACAO,
                DisciplinaId = compensacaoDeAusencia.ComponenteCurricular,
                DisciplinasRegenciaIds = null
            });
        }   
        
        protected async Task CriarRegistroFrequencia()
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
            var registrosFrequenciasAlunos = new List<(string CodigoAluno, int QdadeAula, TipoFrequencia TipoFrequencia)>
            {
                new(CODIGO_ALUNO_1, QUANTIDADE_AULA_4, TipoFrequencia.F),
                new(CODIGO_ALUNO_2, QUANTIDADE_AULA_4, TipoFrequencia.C),
                new(CODIGO_ALUNO_3, QUANTIDADE_AULA_4, TipoFrequencia.R),
                new(CODIGO_ALUNO_4, QUANTIDADE_AULA_4, TipoFrequencia.F)
            };

            foreach (var registroFrequenciaAluno in registrosFrequenciasAlunos)
            {
                await CriarRegistrosFrequenciasAlunos(registroFrequenciaAluno.CodigoAluno, registroFrequenciaAluno.QdadeAula,
                    registroFrequenciaAluno.TipoFrequencia);
            }
        }        
        
        protected async Task CriarRegistrosFrequenciasAlunos(string codigoAluno, int numeroAula,
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
        
        protected async Task CriarFrequenciasAlunos(int bimestre, string disciplinaId)
        {
            var periodosEscolares = ObterTodos<PeriodoEscolar>();
            var periodoEscolar = periodosEscolares.FirstOrDefault(c => c.Bimestre == bimestre);
            
            if (periodoEscolar.EhNulo())
                return;

            const int totalAulas = 22;

            // -> Item1 = Código aluno
            //    Item2 = Total de ausências
            //    Item3 = Total de compensações
            //    Item4 = Total de presenças
            //    Item5 = Total remotos
            var frequenciasAlunos = new List<(string CodigoAluno, int TotalAusencias, int TotalCompensacoes, int TotalAulas, int TotalRemotos)>
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
                    TotalAusencias = frequenciaAluno.TotalAusencias,
                    TotalCompensacoes = frequenciaAluno.TotalCompensacoes,
                    PeriodoEscolarId = periodoEscolar.Id,
                    TotalPresencas = frequenciaAluno.TotalAulas,
                    TotalRemotos = frequenciaAluno.TotalRemotos,
                    DisciplinaId = disciplinaId,
                    CodigoAluno = frequenciaAluno.CodigoAluno,
                    TurmaId = TURMA_CODIGO_1,
                    Tipo = TipoFrequenciaAluno.PorDisciplina,
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });
            }
        }
        
        protected static async Task<CompensacaoDeAusenciaDBDto> ObterCompensacaoDeAusencia(string perfil, int bimestre,
            Modalidade modalidade, ModalidadeTipoCalendario modalidadeTipoCalendario, string componenteCurricular,
            string anoTurma, int quantidadeAulas, bool criarPeriodoEscolar, bool criarPeriodoAbertura, 
            bool permiteCompensacaoForaPeriodoAtivo, bool considerarAnoAnterior)
        {
            return await Task.FromResult(new CompensacaoDeAusenciaDBDto
            {
                Perfil = perfil,
                Bimestre = bimestre,
                Modalidade = modalidade,
                TipoCalendario = modalidadeTipoCalendario,
                ComponenteCurricular = componenteCurricular,
                TipoCalendarioId = TIPO_CALENDARIO_1,
                AnoTurma = anoTurma,
                QuantidadeAula = quantidadeAulas,
                CriarPeriodoEscolar = criarPeriodoEscolar,
                CriarPeriodoAbertura = criarPeriodoAbertura,
                PermiteCompensacaoForaPeriodoAtivo = permiteCompensacaoForaPeriodoAtivo,
                ConsiderarAnoAnterior = considerarAnoAnterior
            });
        }        
    }
}