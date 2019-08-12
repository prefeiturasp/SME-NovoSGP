namespace SME.SGP.Dominio
{
    public class PlanoCiclo : EntidadeBase
    {
        public long Ano { get; set; }
        public long CicloId { get; set; }
        public string Descricao { get; set; }
        public long EscolaId { get; set; }
    }
}