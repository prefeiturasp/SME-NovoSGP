using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Threading.Tasks;
using Xunit;


namespace SME.SGP.TesteIntegracao.Frequencia
{
    public class Ao_inserir_frequencia_professor_cj : FrequenciaTesteBase
    {
        public Ao_inserir_frequencia_professor_cj(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Frequência - Ao registrar frenquecia professor cj ensino fundamental")]
        public async Task Ao_registrar_frenquecia_professor_cj_ensino_fundamental()
        {
            await CriarDadosBasicos(ObterPerfilCJ(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2, DATA_02_05, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),false, TIPO_CALENDARIO_1,true, NUMERO_AULAS_3);
            await CriarAtribuicaoCJ(Modalidade.Fundamental, COMPONENTE_CURRICULAR_PORTUGUES_ID_138);

            await InserirFrequenciaUseCaseComValidacaoBasica(ObterFrequenciaDto());
        }

        [Fact(DisplayName = "Frequência - Ao registrar frequencia professor CJ ensino infantil")]
        public async Task Ao_registrar_frequencia_professor_CJ_ensino_infantil()
        {
            await CriarDadosBasicos(ObterPerfilCJ(), Modalidade.EducacaoInfantil, ModalidadeTipoCalendario.Infantil, DATA_02_05, DATA_07_08, BIMESTRE_2, DATA_02_05, COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString(), criarPeriodo:false);
            await CriarAtribuicaoCJ(Modalidade.EducacaoInfantil, COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213);

            await InserirFrequenciaUseCaseComValidacaoBasica(ObterFrequenciaDto());
        }

    }
}
