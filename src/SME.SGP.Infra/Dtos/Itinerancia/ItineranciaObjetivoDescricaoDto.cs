using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class ItineranciaObjetivoDescricaoDto
    {
        public ItineranciaObjetivoDescricaoDto(string nome, string descricao)
        {
            Nome = nome;
            Descricao = descricao;
        }

        public string Nome { get; set; }
        public string Descricao { get; set; }

        public static implicit operator ItineranciaObjetivoDescricaoDto(ItineranciaObjetivoDto dto)
            => new ItineranciaObjetivoDescricaoDto(dto.Nome, dto.Descricao);
    }
}
