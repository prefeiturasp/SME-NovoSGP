using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dto
{
    public class AtribuicaoSupervisorUEDto
    {
        [Required(ErrorMessage = "O id da dre deve ser informado")]
        public string DreId { get; set; }

        [Required(ErrorMessage = "O id do supervisor deve ser informado")]
        public string SupervisorId { get; set; }

        public List<string> UESIds { get; set; }
    }
}