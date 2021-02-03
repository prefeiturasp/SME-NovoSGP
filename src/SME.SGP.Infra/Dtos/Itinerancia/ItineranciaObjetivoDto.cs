namespace SME.SGP.Infra
{
    public class ItineranciaObjetivoDto
    {
        public ItineranciaObjetivoDto(long id, string nome, bool temDescricao, bool permiteVariasUes, bool selecionado, string descricao)
        {
            Id = id;
            Nome = nome;
            TemDescricao = temDescricao;
            PermiteVariasUes = permiteVariasUes;
            Selecionado = selecionado;
            Descricao = descricao;
        }
        public long Id { get; set; }
        public long RegistroItineranciaObjetivoId { get; set; }
        public string Nome { get; set; }
        public bool TemDescricao { get; set; }
        public bool PermiteVariasUes { get; set; }
        public bool Selecionado { get; set; }
        public string Descricao { get; set; }
    }
}
