using System;

public class SalvarImportacaoLogDto
{
    public SalvarImportacaoLogDto(string nomeArquivo, string tipoArquivoImportacao, string statusImportacao)
    {
        NomeArquivo = nomeArquivo;
        TipoArquivoImportacao = tipoArquivoImportacao;
        StatusImportacao = statusImportacao;
    }

    public long Id { get; set; }
    public string NomeArquivo { get; set; }
    public string TipoArquivoImportacao { get; set; }
    public DateTime DataInicioProcessamento { get; set; }
    public DateTime? DataFimProcessamento { get; set; }
    public long? TotalRegistros { get; set; }
    public long? RegistrosProcessados { get; set; }
    public long? RegistrosComFalha { get; set; }
    public string StatusImportacao { get; set; }
}
