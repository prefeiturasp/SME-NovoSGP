namespace SME.SGP.Infra
{
    public class RegistroItineranciaObjetivoDto
    {
        public RegistroItineranciaObjetivoDto(long id, string nome, bool temDescricao, bool permiteVariasUes)
        {
            Id = id;
            Nome = nome;
            TemDescricao = temDescricao;
            PermiteVariasUes = permiteVariasUes;
        }
        public long Id { get; set; }
        public string Nome { get; set; }
        public bool TemDescricao { get; set; }
        public bool PermiteVariasUes { get; set; }
    }
}
