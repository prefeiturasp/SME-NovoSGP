using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Infra
{
    public class PrimeiroAcessoDto
    {
        [Required(ErrorMessage = "É necessário informar usuário ou código Rf.")]
        [MinLength(5, ErrorMessage = "O usuário ou código Rf deve conter no mínimo 5 caracteres.")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "É necessario informar o RF do usuário.")]
        [MinLength(7, ErrorMessage = "O RFCPF tem no minimo 7 Digitos")]
        [MaxLength(11, ErrorMessage = "O RFCPF tem no Maximo 11 Digitos")]
        public string RFCPF { get; set; }

        public bool UsuarioExterno { get; set; }

        [Required(ErrorMessage = "É necessário informar a nova senha.")]
        [MinLength(8, ErrorMessage = "A senha deve ter no minimo 8 caracteres.")]
        [MaxLength(12, ErrorMessage = "A senha deve ter no máximo 12 caracteres.")]
        public string NovaSenha { get; set; }

        [Compare(nameof(NovaSenha), ErrorMessage = "As senhas não são iguais")]
        public string ConfirmarSenha { get; set; }
    }
}
