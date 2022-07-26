using System;
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

        private async Task CriarFrequenciaAluno(int bimestre)
        {
        }
        
        private async Task<CompensacaoDeAusenciaDBDto> ObterCompensacaoDeAusencia(string perfil, Modalidade modalidade, 
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