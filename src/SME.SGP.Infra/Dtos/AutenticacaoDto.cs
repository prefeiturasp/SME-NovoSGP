using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class AutenticacaoDto
    {
        [Required(ErrorMessage = "É necessário informar usuário ou código Rf.")]
        [MinLength(5, ErrorMessage = "O usuário ou código Rf deve conter no mínimo 5 caracteres.")]
        public string Login { get; set; }

        [Required(ErrorMessage = "É necessário informar a senha.")]
        [MinLength(4, ErrorMessage = "A senha deve conter no mínimo 4 caracteres.")]
        public string Senha { get; set; }
    }
}
