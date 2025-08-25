public class SalvarImportacaoLogErroDto
{
    public SalvarImportacaoLogErroDto(long importacaoLogId, long linhaArquivo, string motivoFalha)
    {
        ImportacaoLogId = importacaoLogId;
        LinhaArquivo = linhaArquivo;
        MotivoFalha = motivoFalha;
    }
    public long ImportacaoLogId { get; set; }
    public long LinhaArquivo { get; set; }
    public string MotivoFalha { get; set; }
}
