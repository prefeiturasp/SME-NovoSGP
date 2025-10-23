namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class PainelEducacionalProficienciaIdepDto
    {
        public int AnoLetivo { get; set; }
        public decimal PercentualInicial { get; set; }
        public decimal PercentualFinal { get; set; }
        public string Boletim { get; set; }
        public ProficienciaIdepResumidoDto Proficiencia { get; set; }
    }
}
