namespace SME.SGP.Infra.Dtos.ImportarArquivo
{
    public class FiltroPesquisaImportacaoDto
    {
        public long? ImportacaoLogId { get; set; }
        public int NumeroPagina { get; set; } = 1;
        public int NumeroRegistros { get; set; } = 10;
    }
}
