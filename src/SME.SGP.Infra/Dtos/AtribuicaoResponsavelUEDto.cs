using SME.SGP.Dominio;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class AtribuicaoResponsavelUEDto
    {
        [Required(ErrorMessage = "A Dre deve ser informada")]
        public string DreId { get; set; }


        [Required(ErrorMessage = "O Supervisor deve ser informado")]
        public string SupervisorId { get; set; }

        public List<string> UESIds { get; set; }
        public TipoResponsavelAtribuicao TipoResponsavelAtribuicao { get; set; }
    }
}