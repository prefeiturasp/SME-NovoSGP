using SME.SGP.Dominio.Constantes;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dominio.Entidades.MapeamentoPap
{
    public static class PapComponenteCurricularMap
    {
        private static readonly IReadOnlyList<(TipoPap Tipo, long ComponenteCurricularId)> _mapeamento;

        static PapComponenteCurricularMap()
        {
            _mapeamento = new List<(TipoPap, long)>
            {
                (TipoPap.PapColaborativo, ComponentesCurricularesConstants.CODIGO_PAP_PROJETO_COLABORATIVO),
                (TipoPap.RecuperacaoAprendizagens, ComponentesCurricularesConstants.CODIGO_PAP_RECUPERACAO_APRENDIZAGENS),
                (TipoPap.Pap2Ano, ComponentesCurricularesConstants.CODIGO_PAP_2_ANO_ALFABETIZACAO),
                (TipoPap.Pap2Ano, ComponentesCurricularesConstants.CODIGO_PAP_2_ANO_COLABORATIVO_ALFABETIZACAO)
            };
        }

        public static IEnumerable<long> ObterComponentesPorTipoPap(TipoPap tipoPap)
        {
            return _mapeamento
                .Where(m => m.Tipo == tipoPap)
                .Select(m => m.ComponenteCurricularId);
        }

        public static TipoPap ObterTipoPapPorComponente(long componenteCurricularId)
        {
            return _mapeamento.FirstOrDefault(m => m.ComponenteCurricularId == componenteCurricularId).Tipo;
        }
    }
}
