namespace SME.SGP.Infra
{
    public class FiltroConselhoClasseConsolidadoDto
    {
        public FiltroConselhoClasseConsolidadoDto(long turmaId, int bimestre, string alunoCodigo)
        {
            TurmaId = turmaId;
            Bimestre = bimestre;
            AlunoCodigo = alunoCodigo;
        }

        public long TurmaId { get; set; }

        public int Bimestre { get; set; }
        public string AlunoCodigo { get; set; }
    }
}
