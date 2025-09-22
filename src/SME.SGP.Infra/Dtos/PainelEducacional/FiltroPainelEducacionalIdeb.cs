using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class FiltroPainelEducacionalIdeb : FiltroPainelEducacionalDreUe
    {
        public int? AnoLetivo { get; set; }
        public PainelEducacionalIdebSerie Serie { get; set; }
    }
}