using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos.PainelEducacional.Reclassificacao
{
    public class PainelEducacionalReclassificacaoDto
    {
        public IEnumerable<ModalidadeReclassificacaoDto> Modalidades { get; set; }
    }
}
