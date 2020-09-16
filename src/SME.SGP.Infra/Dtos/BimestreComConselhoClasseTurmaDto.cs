using System;

namespace SME.SGP.Infra
{
    public class BimestreComConselhoClasseTurmaDto
    {
        public long conselhoClasseId { get; set; }
        public long fechamentoTurmaId { get; set; }
        public long bimestre { get; set; }
        public DateTime periodoFechamentoInicio { get; set; }
        public DateTime periodoFechamentoFim { get; set; }
        public bool periodoAberto { get; set; }
        public long tipoNota { get; set; }
        public float media { get; set; }
        public long anoLetivo { get; set; }
    }
}