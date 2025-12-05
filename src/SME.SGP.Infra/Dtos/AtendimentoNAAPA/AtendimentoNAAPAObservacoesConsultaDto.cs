using System;

namespace SME.SGP.Infra
{
    public class AtendimentoNAAPAObservacoesConsultaDto
    {
        public long IdObservacao { get; set; }
        public string Observacao { get; set; }
        public bool Proprietario { get; set; }
        public DateTime? AlteradoEm { get; set; }
        public string AlteradoPor { get; set; }
        public string AlteradoRF { get; set; }
        public DateTime CriadoEm { get; set; }
        public string CriadoPor { get; set; }
        public string CriadoRF { get; set; }
    }
}
