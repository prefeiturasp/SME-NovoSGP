using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class AlterarEmailDto
    {
        [Required(ErrorMessage = "O nome e-mail deve ser informado.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string NovoEmail { get; set; }
    }
}