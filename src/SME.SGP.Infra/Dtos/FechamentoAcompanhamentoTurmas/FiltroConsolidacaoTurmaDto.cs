using System;

namespace SME.SGP.Infra
{
    public class FiltroConsolidacaoTurmaDto
    {
        public FiltroConsolidacaoTurmaDto(string turmaCodigo, int? bimestre, int pagina = 1, int? anoLetivo = null)
        {
            TurmaCodigo = turmaCodigo;
            Bimestre = bimestre;
            Pagina = pagina;
            AnoLetivo = anoLetivo ?? DateTime.Now.Year;
        }

        public string TurmaCodigo { get; }
        public int? Bimestre { get; }
        public int Pagina { get; set; }
        public int AnoLetivo { get; set; }
    }
}
