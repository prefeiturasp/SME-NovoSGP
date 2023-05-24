namespace SME.SGP.Infra
{
    public class ConselhoClasseSinteseDto
    {
        public long ConselhoClasseId { get; set; }
        public long FechamentoTurmaId { get; set; }
        public string AlunoCodigo { get; set; }
        public string CodigoTurma { get; set; }
        public int Bimestre { get; set; }

        public ConselhoClasseSinteseDto(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo, string codigoTurma, int bimestre)
        {
            ConselhoClasseId = conselhoClasseId;
            FechamentoTurmaId = fechamentoTurmaId;
            AlunoCodigo = alunoCodigo;
            CodigoTurma = codigoTurma;
            Bimestre = bimestre;
        }
    }
}