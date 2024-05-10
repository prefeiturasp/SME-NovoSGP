namespace SME.SGP.Dominio
{
    public class ClassificacaoDocumento
    {
        public long Id { get; set; }
        public long TipoDocumentoId { get; set; }
        public TipoDocumento TipoDocumento { get; set; }
        public string Descricao { get; set; }
        public bool EhRegistroMultiplo { get; set; }
    }
}