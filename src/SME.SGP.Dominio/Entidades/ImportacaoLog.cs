using System;
using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class ImportacaoLog : EntidadeBase
    {
        public string NomeArquivo {  get; set; }
        public string TipoArquivoImportacao { get; set; }
        public DateTime DataInicioProcessamento { get; set; }
        public string StatusImportacao { get; set; }
        public DateTime? DataFimProcessamento { get; set; }
        public long? TotalRegistros { get; set; }
        public long? RegistrosProcessados { get; set; }
        public long? RegistrosComFalha { get; set; }
    }
}
