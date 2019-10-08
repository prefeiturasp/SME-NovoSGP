using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dto
{
    public class AlterarEmailDto
    {
        public string LoginUsuarioASerAlterado { get; set; }

        [Required(ErrorMessage = "O nome e-mail deve ser informado.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string NovoEmail { get; set; }
    }
}