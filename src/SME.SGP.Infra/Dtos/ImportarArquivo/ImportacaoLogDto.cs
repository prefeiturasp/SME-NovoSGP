using Microsoft.AspNetCore.Http;
using System;

public class ImportacaoLogDto
{
    public ImportacaoLogDto(IFormFile arquivo, string tipoArquivoImportacao, string statusImportacao)
    {
        Arquivo = arquivo;
        NomeArquivo = arquivo.FileName;
        TipoArquivoImportacao = tipoArquivoImportacao;
        StatusImportacao = statusImportacao;
    }

    public long Id { get; set; }
    public IFormFile Arquivo { get; set; }
    public string NomeArquivo { get; set; }
    public string TipoArquivoImportacao { get; set; }
    public DateTime DataInicioProcessamento { get; set; }
    public DateTime? DataFimProcessamento { get; set; }
    public long? TotalRegistros { get; set; }
    public long? RegistrosProcessados { get; set; }
    public long? RegistrosComFalha { get; set; }
    public string StatusImportacao { get; set; }
}
