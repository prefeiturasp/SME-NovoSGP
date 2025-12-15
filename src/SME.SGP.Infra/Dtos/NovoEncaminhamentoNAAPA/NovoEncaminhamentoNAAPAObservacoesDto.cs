using System;

namespace SME.SGP.Infra
{
    public class NovoEncaminhamentoNAAPAObservacoesDto
    {
        public long Id { get; set; }
        public string Observacao { get; set; }
        public bool Proprietario { get; set; }
        public AuditoriaDto Auditoria { get; set; }
    }
}
