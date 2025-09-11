namespace SME.SGP.Dominio
{
    public class ConsolidadoAtendimentoNAAPA : EntidadeBase
    {
        public int AnoLetivo { get; set; }
        public int Mes { get; set; }
        public long UeId { get; set; }
        public long Quantidade { get; set; }
        public string NomeProfissional { get; set; }
        public string RfProfissional { get; set; }
        public Modalidade Modalidade { get; set; }
    }
}