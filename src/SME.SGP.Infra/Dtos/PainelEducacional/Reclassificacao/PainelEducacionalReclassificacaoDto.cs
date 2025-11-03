using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos.PainelEducacional.Reclassificacao
{
    public class PainelEducacionalReclassificacaoDto
    {
        public string Modalidade { get; set; }
        public IEnumerable<SerieAnoReclassificacaoDto> SerieAno { get; set; }
    }
}
