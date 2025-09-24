namespace SME.SGP.Infra.Dtos.ImportarArquivo
{
    public class ProficienciaIdebDto : ArquivoImportacaoBaseDto
    {
        public long? Id { get; set; }
        public string CodigoEOLEscola { get; set; }
        public int AnoLetivo { get; set; }
        public string Boletim { get; set; }
    }
}
