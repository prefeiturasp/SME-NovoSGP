using System;

public class ImportacaoLogQueryRetornoDto
{

    public long Id { get; set; }
    public string NomeArquivo { get; set; }
    public string StatusImportacao { get; set; }
    public long? TotalRegistros { get; set; }
    public long? RegistrosProcessados { get; set; }
}
