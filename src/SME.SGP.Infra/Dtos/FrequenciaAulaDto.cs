using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class FrequenciaAulaDto
    {
        public TipoFrequencia TipoFrequencia { get; set; }
        public bool Compareceu { get; set; }
        public int NumeroAula { get; set; }
    }
}