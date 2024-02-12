using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class FiltroGraficoReflexoFrequenciaBuscaAtivaDto : FiltroGraficoBuscaAtivaDto
    {
        [Required]
        public int Mes { get; set; }
    }
}