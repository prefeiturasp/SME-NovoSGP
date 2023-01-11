namespace SME.SGP.Infra
{
    public class TipoDocumentoCompletoDto
    {
        public TipoDocumentoCompletoDto(){}
        
        public long TipoDocumentoId { get; set; }
        public string TipoDocumentoNome { get; set; }
        public long ClassificacaoId { get; set; }
        public string ClassificacaoNome { get; set; }
    }
}
