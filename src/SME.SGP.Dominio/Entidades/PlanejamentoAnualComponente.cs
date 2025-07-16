using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class PlanejamentoAnualComponente : EntidadeBase
    {
        public PlanejamentoAnualComponente()
        {
            ObjetivosAprendizagem = new List<PlanejamentoAnualObjetivoAprendizagem>();
        }

        public PlanejamentoAnualComponente(long componenteCurricularId, string descricao, long planejamentoAnualPeriodoEscolarId)
        {
            ComponenteCurricularId = componenteCurricularId;
            Descricao = descricao;
            PlanejamentoAnualPeriodoEscolarId = planejamentoAnualPeriodoEscolarId;
            ObjetivosAprendizagem = new List<PlanejamentoAnualObjetivoAprendizagem>();
        }

        public long ComponenteCurricularId { get; set; }
        public string Descricao { get; set; }
        public long PlanejamentoAnualPeriodoEscolarId { get; set; }
        public List<PlanejamentoAnualObjetivoAprendizagem> ObjetivosAprendizagem { get; set; }
        public bool Excluido { get; set; }
    }
}
