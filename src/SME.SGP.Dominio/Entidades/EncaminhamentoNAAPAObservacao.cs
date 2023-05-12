namespace SME.SGP.Dominio
{
    public class EncaminhamentoNAAPAObservacao : EntidadeBase
    {
        public EncaminhamentoNAAPA EncaminhamentoNAAPA { get; set; }
        public long EncaminhamentoNAAPAId { get; set; }
        public string Observacao { get; set; }
        public bool Excluido { get; set; }
    }
}