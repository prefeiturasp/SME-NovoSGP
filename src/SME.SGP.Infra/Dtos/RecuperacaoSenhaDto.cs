using System;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class RecuperacaoSenhaDto
    {
        [Required(ErrorMessage = "A nova senha deve ser informada.")]
        [MinLength(8, ErrorMessage = "A senha deve conter no mínimo 8 caracteres")]
        [MaxLength(12, ErrorMessage = "A senha deve conter no máximo 12 caracteres")]
        public string NovaSenha { get; set; }

        [Required(ErrorMessage = "O token de recuperação de senha deve ser informado.")]
        public Guid Token { get; set; }
    }
}