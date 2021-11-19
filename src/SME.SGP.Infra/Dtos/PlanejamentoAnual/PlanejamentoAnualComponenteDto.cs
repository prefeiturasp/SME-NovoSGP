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
    public class PlanejamentoAnualComponenteResumidoDto
    {
        public string DescricaoAtual { get; set; }
        public string DescricaoNovo { get; set; }
    }
}
