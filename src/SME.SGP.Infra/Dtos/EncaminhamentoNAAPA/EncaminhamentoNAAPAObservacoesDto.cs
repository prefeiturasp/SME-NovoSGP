using System;

namespace SME.SGP.Infra
{
    public class EncaminhamentoNAAPAObservacoesDto
    {
        public long IdObservacao { get; set; }
        public string Observacao { get; set; }
        public bool Proprietario { get; set; }
        public AuditoriaDto Auditoria { get; set; }
    }
}
