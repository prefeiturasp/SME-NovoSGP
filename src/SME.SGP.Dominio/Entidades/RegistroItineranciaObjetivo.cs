namespace SME.SGP.Dominio
{
    public class RegistroItineranciaObjetivo : EntidadeBase
    {
        public long RegistroItineranciaObjetivosBaseId { get; set; }
        public long RegistroItineranciaId { get; set; }
        public string Descricao { get; set; }
        public bool Excluido { get; set; }
    }
}