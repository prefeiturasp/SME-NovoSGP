using System;

namespace SME.SGP.Infra
{
    public class ConsolidacaoConselhoClasseAlunoMigracaoDto
    {
        public string AlunoCodigo { get; set; }
        public long TurmaId { get; set; }
        public long DisciplinaId { get; set; }
        public double? Nota { get; set; }
        public long? ConceitoId { get; set; }
        public int Bimestre { get; set; }
        public long? ParecerConclusivoId { get; set; }
        public DateTime CriadoEm { get; set; }
        public string CriadoPor { get; set; }
        public string CriadoRf { get; set; }
        public long ConselhoClasseAlunoId { get; set; }
        public long FechamentoNotaId { get; set; }
        public int Status { get; set; }
    }
}
