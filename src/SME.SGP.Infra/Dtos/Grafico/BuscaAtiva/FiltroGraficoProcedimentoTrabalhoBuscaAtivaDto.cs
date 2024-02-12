using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class FiltroGraficoProcedimentoTrabalhoBuscaAtivaDto: FiltroGraficoBuscaAtivaDto
    {
        [Required]
        public EnumProcedimentoTrabalhoBuscaAtiva TipoProcedimentoTrabalho { get; set; }
    }
}