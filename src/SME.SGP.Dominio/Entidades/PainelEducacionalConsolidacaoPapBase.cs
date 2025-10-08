using System;

namespace SME.SGP.Dominio.Entidades
{
    public class PainelEducacionalConsolidacaoPapBase
    {
        public int Id { get; set; }
        public int AnoLetivo { get; set; }
        public TipoPap TipoPap { get; set; }
        public int TotalTurmas { get; set; }
        public int TotalAlunos { get; set; }
        public int TotalAlunosComFrequenciaInferiorLimite { get; set; }
        public int TotalAlunosDificuldadeTop1 { get; set; }
        public int TotalAlunosDificuldadeTop2 { get; set; }
        public int TotalAlunosDificuldadeOutras { get; set; }
        public string NomeDificuldadeTop1 { get; set; }
        public string NomeDificuldadeTop2 { get; set; }
        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    }
}
