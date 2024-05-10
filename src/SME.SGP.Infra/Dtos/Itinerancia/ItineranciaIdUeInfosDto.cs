using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class ItineranciaIdUeInfosDto
    {
        public string UeNome { get; set; }
        public TipoEscola TipoEscola { get; set; }
        public long ItineranciaId { get; set; }
        public string NomeFormatado { get { return $"{TipoEscola.ShortName()} {UeNome}"; } }

    }
}
