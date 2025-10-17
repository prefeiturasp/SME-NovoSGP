using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoUe
{
    public class FiltroPainelEducacionalNotasVisaoUe
    {
        public string CodigoDre { get; set; }
        public int AnoLetivo { get; set; } 
        public Modalidade Modalidade { get; set; }
        public int Bimestre { get; set; }
    }
}
