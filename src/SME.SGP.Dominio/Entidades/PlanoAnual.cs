namespace SME.SGP.Dominio
{
    public class PlanoAnual : EntidadeBase
    {
        public int Ano { get; set; }
        public string Descricao { get; set; }
        public long EscolaId { get; set; }
        public long TurmaId { get; set; }
    }
}