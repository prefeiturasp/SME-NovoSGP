namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class PainelEducacionalRegistroFluenciaLeitoraDto
    {
        public int IdFluencia { get; set; }
        public int CodigoFluencia { get; set; }
        public int AnoLetivo { get; set; }
        public string DreCodigo { get; set; }
        public string DreNome { get; set; }
        public string UeCodigo { get; set; }
        public string UeNome { get; set; }
        public string TurmaNome { get; set; }
        public int Periodo { get; set; } 
    }
}
