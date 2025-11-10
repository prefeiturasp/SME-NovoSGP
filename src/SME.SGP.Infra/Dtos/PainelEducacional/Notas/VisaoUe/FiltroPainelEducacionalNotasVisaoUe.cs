using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoUe
{
    public class FiltroPainelEducacionalNotasVisaoUe
    {
        public string CodigoUe { get; set; }
        public int AnoLetivo { get; set; } 
        public Modalidade Modalidade { get; set; }
        public int Bimestre { get; set; }
        public int NumeroPagina { get; set; }
        public int NumeroRegistros { get; set; }
    }
}
