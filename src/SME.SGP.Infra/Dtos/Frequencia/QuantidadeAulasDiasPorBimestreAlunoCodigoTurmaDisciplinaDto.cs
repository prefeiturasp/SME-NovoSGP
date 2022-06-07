using System;

namespace SME.SGP.Infra
{
    public class QuantidadeAulasDiasPorBimestreAlunoCodigoTurmaDisciplinaDto
    {
        public int TotalAulas { get; set; }
        public int AulasId { get; set; }
        public int TipoFrequencia { get; set; }
        public DateTime DataAula { get; set; }
    }
}
