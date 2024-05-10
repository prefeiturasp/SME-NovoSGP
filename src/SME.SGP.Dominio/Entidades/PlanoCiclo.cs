namespace SME.SGP.Dominio
{
    public class PlanoCiclo : EntidadeBase
    {
        public int Ano { get; set; }
        public long CicloId { get; set; }
        public string Descricao { get; set; }
        public string EscolaId { get; set; }
        public bool Migrado { get; set; }
    }
}