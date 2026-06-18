namespace SME.SGP.Dominio
{
    public class NotificacaoCompensacaoAusencia
    {
        public long Id { get; set; }
        public long NotificacaoId { get; set; }
        public long CompensacaoAusenciaId { get; set; }
    }
}
