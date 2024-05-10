using SME.SGP.Dominio;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class AtribuicaoResponsavelUEDto
    {
        [Required(ErrorMessage = "A Dre deve ser informada")]
        public string DreId { get; set; }


        [Required(ErrorMessage = "O Responsavel deve ser informado")]
        public string ResponsavelId { get; set; }

        public List<string> UesIds { get; set; }


        [Required(ErrorMessage = "O Tipo da Atribuição deve ser informado")]
        public TipoResponsavelAtribuicao TipoResponsavelAtribuicao { get; set; }
    }
}