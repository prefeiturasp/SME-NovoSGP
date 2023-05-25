namespace SME.SGP.Dominio
{
    public class ConsolidadoAtendimentoNAAPA : EntidadeBase
    {
        public int AnoLetivo { get; set; }
        public int Mes { get; set; }
        public long UeId { get; set; }
        public long Quantidade { get; set; }
        public string Profissional { get; set; }
    }
}