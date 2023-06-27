using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class MensagemConsolidarTurmaConselhoClasseAlunoPorUeAnoDto
    {
        public MensagemConsolidarTurmaConselhoClasseAlunoPorUeAnoDto(long ueId, int anoLetivo)
        {
            this.UeId = ueId;
            this.AnoLetivo = anoLetivo;
        }

        public long UeId { get; set; }
        public int AnoLetivo { get; set; }
    }
}
