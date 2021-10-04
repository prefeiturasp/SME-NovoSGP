using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class FiltroDashboardAcompanhamentoAprendizagemPorDreDto
    {
        [Required]
        public int AnoLetivo { get; set; }
        public int? Semestre { get; set; }
    }
}
