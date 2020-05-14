using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio
{
    public class WfAprovacaoNotaFechamento
    {
        public long Id { get; set; }
        public long WfAprovacaoId { get; set; }
        public WorkflowAprovacao WfAprovacao { get; set; }
        public long FechamentoNotaId { get; set; }
        public FechamentoNota FechamentoNota { get; set; }

        public double? Nota { get; set; }
        public long? ConceitoId { get; set; }
        public Conceito Conceito { get; set; }
    }
}
