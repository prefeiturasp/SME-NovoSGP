using System;

namespace SME.SGP.Infra
{
    public class QuantidadeAulasDiasPorBimestreAlunoCodigoTurmaDisciplinaDto
    {
        public long TotalAulasNoDia { get; set; }
        public DateTime DataAula { get; set; }
        public long AulasId { get; set; }
        public int TipoFrequencia { get; set; }
        public string AlunoCodigo { get; set; }
        public long AnotacaoId { get; set; }
        public string MotivoAusencia { get; set; }
        public int TotalPresencas { get; set; }
        public int TotalAusencias { get; set; }
        public int TotalRemotos { get; set; }
    }
}
