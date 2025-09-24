namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class PainelEducacionalRegistroFluenciaLeitoraAgrupamentoFluenciaDto
    {
        public string Fluencia { get; set; }
        public string DescricaoFluencia { get; set; }
        public decimal Percentual { get; set; }
        public int QuantidadeAluno { get; set; }
        public int AnoLetivo { get; set; }
        public int Periodo { get; set; }
        public string DreCodigo { get; set; }
        public string DreNome { get; set; }
        public string UeCodigo { get; set; }
        public string UeNome { get; set; }
    }
}
