namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class ProficienciaIdepAgrupadaDto
    {
        public int AnoLetivo { get; set; }
        public int ComponenteCurricular { get; set; }
        public long EtapaEnsino { get; set; } 
        public decimal ProficienciaMedia { get; set; }
        public string Boletim { get; set; }
    }
}
