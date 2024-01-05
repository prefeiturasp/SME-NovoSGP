using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
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
            await CriarDadosBasicosVigenciaRelativa(ObterPerfilCJ(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, BIMESTRE_2, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),false, TIPO_CALENDARIO_1,false, NUMERO_AULAS_3);
            await CriarPeriodoEscolarCustomizadoQuartoBimestre(true);
            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);
            await CriarAtribuicaoCJ(Modalidade.Fundamental, COMPONENTE_CURRICULAR_PORTUGUES_ID_138);

            await InserirFrequenciaUseCaseComValidacaoBasica(ObterFrequenciaDto());
        }

        [Fact(DisplayName = "Frequência - Ao registrar frequencia professor CJ ensino infantil")]
        public async Task Ao_registrar_frequencia_professor_CJ_ensino_infantil()
        {
            await CriarDadosBasicosVigenciaRelativa(ObterPerfilCJ(), Modalidade.EducacaoInfantil, ModalidadeTipoCalendario.Infantil, BIMESTRE_2, COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString(), criarPeriodo:false);
            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);
            await CriarAtribuicaoCJ(Modalidade.EducacaoInfantil, COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213);

            await InserirFrequenciaUseCaseComValidacaoBasica(ObterFrequenciaDto());
        }

    }
}
