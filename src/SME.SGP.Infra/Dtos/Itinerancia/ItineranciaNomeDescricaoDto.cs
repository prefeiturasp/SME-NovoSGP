using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class ItineranciaNomeDescricaoDto
    {        
        public string Nome { get; set; }
        public string Descricao { get; set; }

        public long Id { get; set; }
        public long ItineranciaObjetivoBaseId { get; set; }
        public bool TemDescricao { get; set; }       
    }
}
