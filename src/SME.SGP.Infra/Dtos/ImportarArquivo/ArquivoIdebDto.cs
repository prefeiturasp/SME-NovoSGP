namespace SME.SGP.Infra.Dtos.ImportarArquivo
{
    public class ArquivoIdebDto : ArquivoImportacaoBaseDto
    {
        public ArquivoIdebDto(int serieAno, string codigoEOLEscola, decimal nota)
        {
            SerieAno = serieAno;
            CodigoEOLEscola = codigoEOLEscola;
            Nota = nota;
        }
        public int SerieAno { get; set; }
        public string CodigoEOLEscola { get; set; }
        public decimal Nota { get; set; }
    }
}
