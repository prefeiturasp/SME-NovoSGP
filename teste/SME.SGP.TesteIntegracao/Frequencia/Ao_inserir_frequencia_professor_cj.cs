using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Threading.Tasks;
using Xunit;


namespace SME.SGP.TesteIntegracao.Frequencia
{
    public class Ao_inserir_frequencia_professor_cj : FrequenciaBase
    {
        private DateTime DATA_02_05 = new(DateTimeExtension.HorarioBrasilia().Year, 05, 02);
        private DateTime DATA_07_08 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 08);

        public Ao_inserir_frequencia_professor_cj(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        //[Fact]
        public async Task Ao_registrar_frenquecia_professor_cj_ensino_fundamental()
        {
            await CriarDadosBasicos(ObterPerfilCJ(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2, DATA_02_05, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), NUMERO_AULAS_3, false);
            await CriarAtribuicaoCJ(Modalidade.Fundamental, COMPONENTE_CURRICULAR_PORTUGUES_ID_138);

            await InserirFrequenciaUseCaseComValidacaoBasica(ObtenhaFrenqueciaDto());
        }
    }
}
