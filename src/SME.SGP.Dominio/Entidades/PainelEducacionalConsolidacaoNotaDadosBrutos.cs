namespace SME.SGP.Dominio.Entidades
{
    public class PainelEducacionalConsolidacaoNotaDadosBrutos
    {
        public string TurmaNome { get; set; }
        public char AnoTurma { get; set; }
        public short AnoLetivo { get; set; }
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public short Bimestre { get; set; }
        public Modalidade Modalidade { get; set; }
        public int IdComponenteCurricular { get; set; }
        public int? Nota { get; set; }
        public string ValorConceito { get; set; }
        public bool ConceitoDeAprovado { get; set; }
        public int ValorMedioNota { get; set; }
    }
}