namespace SME.SGP.Infra
{
    public class ItineranciaObjetivoDto
    {        
        public long Id { get; set; }
        public long ItineranciaObjetivoId { get; set; }
        public string Nome { get; set; }
        public bool TemDescricao { get; set; }
        public bool PermiteVariasUes { get; set; }        
        public string Descricao { get; set; }
    }
}
