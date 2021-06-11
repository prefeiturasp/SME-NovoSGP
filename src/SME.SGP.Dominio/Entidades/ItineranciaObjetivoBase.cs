namespace SME.SGP.Dominio
{
    public class ItineranciaObjetivoBase
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public bool TemDescricao { get; set; }
        public int Ordem { get; set; }
        public bool Excluido { get; set; }
    }
}
