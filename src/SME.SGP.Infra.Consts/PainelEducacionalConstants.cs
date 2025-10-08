using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using System.Collections.Generic;

namespace SME.SGP.Infra.Consts
{
    public static class PainelEducacionalConstants
    {
        public const string NOME_OUTRAS_DIFICULDADES_PAP = "OUTRAS";
        public const int ID_OUTRAS_DIFICULDADES_PAP = 0;
        public const int ANO_LETIVO_MIM_LIMITE = 2019;
        public const double PERCENTUAL_MAX_FREQUENCIA_INDICADORES_PAP = 75;
        public const int QTD_INDICADORES_PAP = 2;
        public static readonly Dictionary<TipoPap, string> COD_COMPONENTES_CURRICULARES_PARA_INDICADORES_PAP
             = new Dictionary<TipoPap, string>
            {
                { TipoPap.PapColaborativo, ComponentesCurricularesConstants.CODIGO_PAP_PROJETO_COLABORATIVO.ToString() },
                { TipoPap.RecuperacaoAprendizagens, ComponentesCurricularesConstants.CODIGO_PAP_RECUPERACAO_APRENDIZAGENS.ToString() },
                { TipoPap.Pap2Ano, string.Join(',', new[] { ComponentesCurricularesConstants.CODIGO_PAP_2_ANO_ALFABETIZACAO,
                                         ComponentesCurricularesConstants.CODIGO_PAP_2_ANO_COLABORATIVO_ALFABETIZACAO }) }
            };
    }
}
