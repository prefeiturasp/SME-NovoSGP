namespace SME.SGP.Infra
{
    public class DadosDownloadRelatorioDto : DadosRelatorioDto
    {
        public string ContentType { get; set; }
        public string NomeArquivo { get; set; }
    }
}
