namespace SME.SGP.Infra
{
    public class ConsolidacaoConselhoClasseAlunoMigracaoDto
    {
        public long ConsolidacaoId { get; set; }
        public string AlunoCodigo { get; set; }
        public long TurmaId { get; set; }
        public int Bimestre { get; set; }
    }
}
