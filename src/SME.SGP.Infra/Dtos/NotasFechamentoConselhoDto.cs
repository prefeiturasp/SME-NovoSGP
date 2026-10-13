using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class NotasFechamentoConselhoDto
    {
        public IEnumerable<NotaConceitoBimestreComponenteDto> Fechamento { get; set; }
        public IEnumerable<NotaConceitoBimestreComponenteDto> Conselho { get; set; }
    }
}
