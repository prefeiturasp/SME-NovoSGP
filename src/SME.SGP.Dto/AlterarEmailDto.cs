using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dto
{
    public class AlterarEmailDto
    {
        [Required(ErrorMessage = "O nome e-mail deve ser informado.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string NovoEmail { get; set; }
    }
}