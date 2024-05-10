using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class MensagemConsolidarTurmaConselhoClasseAlunoPorUeDto
    {
        public MensagemConsolidarTurmaConselhoClasseAlunoPorUeDto(string codigoUe, int anoLetivo)
        {
            this.CodigoUe = codigoUe;
            this.AnoLetivo = anoLetivo;
        }

        public string CodigoUe { get; set; }
        public int AnoLetivo { get; set; }
    }
}
