using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class PlanejamentoAnualComponenteDto
    {
        public long ComponenteCurricularId { get; set; }
        public string Descricao { get; set; }
        public IEnumerable<long> ObjetivosAprendizagemId { get; set; }
        public AuditoriaDto Auditoria { get; set; }
    }
}
