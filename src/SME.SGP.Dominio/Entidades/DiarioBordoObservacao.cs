namespace SME.SGP.Dominio
{
    public class DiarioBordoObservacao : EntidadeBase
    {
        public string Observacao { get; set; }
        public long DiarioBordoId { get; set; }
        public long UsuarioId { get; set; }
        public bool Excluido { get; set; }
    }
}
