using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class FrequenciaDetalheAulaDto
    {
        public TipoFrequencia Tipo { get; set; }
        public string TipoFrequencia { get => Tipo.ShortName(); }
        public int NumeroAula { get; set; }
    }
}
