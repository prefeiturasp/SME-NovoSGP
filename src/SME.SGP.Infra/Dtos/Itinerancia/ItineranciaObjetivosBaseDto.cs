namespace SME.SGP.Infra
{
    public class ItineranciaObjetivosBaseDto
    {
        public ItineranciaObjetivosBaseDto(long id, string nome, bool temDescricao)
        {
            ItineranciaObjetivoBaseId = id;
            Nome = nome;
            TemDescricao = temDescricao;
        }
        public long ItineranciaObjetivoBaseId { get; set; }
        public string Nome { get; set; }
        public bool TemDescricao { get; set; }
    }
}
