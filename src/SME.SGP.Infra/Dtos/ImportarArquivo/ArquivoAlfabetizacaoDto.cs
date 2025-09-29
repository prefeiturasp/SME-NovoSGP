namespace SME.SGP.Infra.Dtos.ImportarArquivo
{
    public class ArquivoAlfabetizacaoDto : ArquivoImportacaoBaseDto
    {
        public ArquivoAlfabetizacaoDto(string codigoEOLEscola, decimal taxaAlfabetizacao, int anoLetivo)
        {
            this.CodigoEOLEscola = codigoEOLEscola;
            this.TaxaAlfabetizacao = taxaAlfabetizacao;
            this.AnoLetivo = anoLetivo;
        }

        public int AnoLetivo { get; set; }
        public decimal TaxaAlfabetizacao { get; set; }
    }
}
