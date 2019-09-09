using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dto
{
    public class AtribuicaoSupervisorUEDto
    {
        [Required(ErrorMessage = "A Dre deve ser informada")]
        public string DreId { get; set; }

        [Required(ErrorMessage = "O Supervisor deve ser informado")]
        public string SupervisorId { get; set; }

        [ListaTemElementos(ErrorMessage = "Deve informar ao menos uma ue")]
        public List<string> UESIds { get; set; }
    }
}