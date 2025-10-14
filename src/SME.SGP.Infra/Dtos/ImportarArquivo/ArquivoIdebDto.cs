namespace SME.SGP.Infra.Dtos.ImportarArquivo
{
    public class ArquivoIdebDto : ArquivoImportacaoBaseDto
    {
        public ArquivoIdebDto(int serieAno, string codigoEOLEscola, decimal nota, int anoLetivo)
        {
            SerieAno = serieAno;
            AnoLetivo = anoLetivo;   
            CodigoEOLEscola = codigoEOLEscola;
            Nota = nota;
        }
        public int SerieAno { get; set; }
        public int AnoLetivo { get; set; }
        public decimal Nota { get; set; }
    }
}
