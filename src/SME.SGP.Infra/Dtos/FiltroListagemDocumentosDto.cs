namespace SME.SGP.Dto
{
    public class FiltroListagemDocumentosDto
    {
        public long? DreId { get; set; }
        public long? UeId { get; set; }
        public long TipoDocumentoId { get; set; }
        public long ClassificacaoId { get; set; }
        public int? AnoLetivo { get; set; }
    }
}