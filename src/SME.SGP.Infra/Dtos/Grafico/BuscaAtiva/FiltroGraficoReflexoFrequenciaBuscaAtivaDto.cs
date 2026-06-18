using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class FiltroGraficoReflexoFrequenciaBuscaAtivaDto : FiltroGraficoBuscaAtivaDto
    {
        [Required]
        public int Mes { get; set; }
    }
}