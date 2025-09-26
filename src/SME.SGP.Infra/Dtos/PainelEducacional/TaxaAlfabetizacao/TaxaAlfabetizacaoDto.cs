namespace SME.SGP.Infra.Dtos.PainelEducacional.TaxaAlfabetizacao
{
    public class TaxaAlfabetizacaoDto
    {
        public string CodigoDre { get; set; }
        public string CodigoUe { get; set; }
        public int AnoLetivo { get; set; }
        public string CodigoEOLEscola { get; set; }
        public decimal Taxa { get; set; }
    }
}
