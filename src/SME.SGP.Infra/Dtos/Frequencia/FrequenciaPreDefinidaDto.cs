using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class FrequenciaPreDefinidaDto
    {
        public string CodigoAluno { get; set; }
        public string TipoFrequencia { get => Tipo.ShortName(); }
        public TipoFrequencia Tipo { get; set; }
            
    }
}
