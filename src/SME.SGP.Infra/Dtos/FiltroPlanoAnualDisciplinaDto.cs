using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class FiltroPlanoAnualDisciplinaDto : FiltroPlanoAnualDto
    {
        [Required(ErrorMessage = "Componente curricular deve ser informado para filtrar os objetivos")]
        public long DisciplinaId { get; set; }
    }
}
