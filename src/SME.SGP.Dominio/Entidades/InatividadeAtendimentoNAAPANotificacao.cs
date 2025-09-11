namespace SME.SGP.Dominio
{
    public class InatividadeAtendimentoNAAPANotificacao : EntidadeBase
    {
        public long EncaminhamentoNAAPAId { get; set; }
        public long NotificacaoId { get; set; }
        public bool Excluido { get; set; }
    }
}
