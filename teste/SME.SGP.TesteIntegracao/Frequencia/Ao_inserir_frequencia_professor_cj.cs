﻿using SME.SGP.Dominio;
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

        //[Fact]
        public async Task Ao_registrar_frenquecia_professor_cj_ensino_fundamental()
        {
            await CriarDadosBasicos(ObterPerfilCJ(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2, DATA_02_05, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),true, TIPO_CALENDARIO_1,false, NUMERO_AULAS_3);
            await CriarAtribuicaoCJ(Modalidade.Fundamental, COMPONENTE_CURRICULAR_PORTUGUES_ID_138);

            await InserirFrequenciaUseCaseComValidacaoBasica(ObtenhaFrenqueciaDto());
        }

        //[Fact]
        public async Task Ao_registrar_frequencia_professor_CJ_ensino_infantil()
        {
            await CriarDadosBasicos(ObterPerfilCJ(), Modalidade.EducacaoInfantil, ModalidadeTipoCalendario.Infantil, DATA_02_05, DATA_07_08, BIMESTRE_2, DATA_02_05, COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString());
            await CriarAtribuicaoCJ(Modalidade.EducacaoInfantil, COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213);

            await InserirFrequenciaUseCaseComValidacaoBasica(ObtenhaFrenqueciaDto());
        }

    }
}
