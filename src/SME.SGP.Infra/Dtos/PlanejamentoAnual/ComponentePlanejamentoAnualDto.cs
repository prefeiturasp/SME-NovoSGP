using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class ComponentePlanejamentoAnualDto
    {
        public long ComponenteCurricularId { get; set; }
        public string Descricao { get; set; }
        public IEnumerable<long> ObjetivosAprendizagemId { get; set; }
    }
}
