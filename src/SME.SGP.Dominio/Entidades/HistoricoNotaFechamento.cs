namespace SME.SGP.Dominio
{
    public class HistoricoNotaFechamento
    {
        public long HistoricoNotaId { get; set; }
        public long Id {get; set;}
        public long FechamentoNotaId { get; set; }
        public long? WorkFlowId { get; set; }
    }
}
