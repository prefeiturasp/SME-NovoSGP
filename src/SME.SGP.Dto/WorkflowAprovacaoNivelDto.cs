using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Dto
{
    public class WorkflowAprovacaoNivelDto
    {
        [Required(ErrorMessage = "É necessário informar o nível.")]
        public int Nivel { get; set; }

        [Required(ErrorMessage = "É necessário informar o usuário.")]
        [MinLength(1, ErrorMessage = "É necessário informar o usuário.")]
        public string UsuarioId { get; set; }

        [Required(ErrorMessage = "É necessário informar a descrição do nível.")]
        [MinLength(3, ErrorMessage = "Descrição do nível deve conter no mínimo 3 caracteres.")]
        public string Descricao { get; set; }

    }
}
