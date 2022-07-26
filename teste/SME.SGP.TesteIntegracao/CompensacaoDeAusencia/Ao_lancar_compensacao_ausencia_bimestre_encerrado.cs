using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.CompensacaoDeAusencia.Base;
using SME.SGP.TesteIntegracao.Setup;

namespace SME.SGP.TesteIntegracao.CompensacaoDeAusencia
{
    public class Ao_lancar_compensacao_ausencia_bimestre_encerrado : CompensacaoDeAusenciaTesteBase
    {
        public Ao_lancar_compensacao_ausencia_bimestre_encerrado(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        
        public async Task Deve_bloquear_lancar_compensacao_ausencia_componente_que_nao_lanca_frequencia()
        {
            await CriarPeriodoEscolar();
            await CriarFrequenciasAlunos(BIMESTRE_1, COMPONENTE_LINGUA_PORTUGUESA_ID_138);

            var periodosEscolares = ObterTodos<PeriodoEscolar>();

            var dataInicioPrimeiroBimestre =
                periodosEscolares.FirstOrDefault(c => c.Bimestre == BIMESTRE_1)!.PeriodoInicio;

            var compensacaoDeAusencia = await ObterCompensacaoDeAusencia(ObterPerfilProfessor(),
                Modalidade.Fundamental,
                ModalidadeTipoCalendario.FundamentalMedio,
                COMPONENTE_CURRICULAR_AULA_COMPARTILHADA.ToString(),
                ANO_7,
                dataInicioPrimeiroBimestre,
                20);
            
            await CriarDadosBase(compensacaoDeAusencia);
        }

        private async Task CriarPeriodoEscolar()
        {
            await CriarPeriodoEscolar(DATA_03_01_INICIO_BIMESTRE_1, DATA_29_04_FIM_BIMESTRE_1, BIMESTRE_1);
            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_08_07_FIM_BIMESTRE_2, BIMESTRE_2);
            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_30_09_FIM_BIMESTRE_3, BIMESTRE_3);
            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4);            
        }

        private async Task CriarFrequenciasAlunos(int bimestre, string disciplinaId)
        {
            var periodosEscolares = ObterTodos<PeriodoEscolar>();
            var periodoEscolar = periodosEscolares.FirstOrDefault(c => c.Bimestre == bimestre);
            
            if (periodoEscolar == null)
                return;
            ;
            const int totalAulas = 22;

            var frequenciasAlunos = new List<Tuple<string, int, int, int, int>>
            {
                new(CODIGO_ALUNO_1, TOTAL_AUSENCIAS_1, TOTAL_COMPENSACOES_1, totalAulas - TOTAL_AUSENCIAS_1, TOTAL_REMOTOS_0),
                new(CODIGO_ALUNO_2, TOTAL_AUSENCIAS_3, TOTAL_COMPENSACOES_4, totalAulas - TOTAL_AUSENCIAS_3, TOTAL_REMOTOS_0),
                new(CODIGO_ALUNO_3, TOTAL_AUSENCIAS_7, TOTAL_COMPENSACOES_6, totalAulas - TOTAL_AUSENCIAS_7, TOTAL_REMOTOS_0),
                new(CODIGO_ALUNO_4, TOTAL_AUSENCIAS_8, TOTAL_COMPENSACOES_1, totalAulas - TOTAL_AUSENCIAS_8, TOTAL_REMOTOS_0)
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
            DateTime dataReferencia, int quantidadeAulas)
        {
            return await Task.FromResult(new CompensacaoDeAusenciaDBDto()
            {
                Perfil = perfil,
                Modalidade = modalidade,
                TipoCalendario = modalidadeTipoCalendario,
                Bimestre = BIMESTRE_1,
                ComponenteCurricular = componenteCurricular,
                TipoCalendarioId = TIPO_CALENDARIO_1,
                AnoTurma = anoTurma,
                DataReferencia = dataReferencia,
                QuantidadeAula = quantidadeAulas
            });
        }        
    }
}