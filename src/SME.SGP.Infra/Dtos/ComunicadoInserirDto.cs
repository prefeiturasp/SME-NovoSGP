using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dto
{
    public class ComunicadoInserirDto
    {
        [DataRequerida(ErrorMessage = "A data de envio é obrigatória.")]
        public DateTime DataEnvio { get; set; }

        public DateTime? DataExpiracao { get; set; }

        [Required(ErrorMessage = "É necessário informar a descrição.")]
        [MinLength(5, ErrorMessage = "A descrição deve conter no mínimo 5 caracteres.")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O grupo do comunicado deve ser informado.")]
        [ListaTemElementos(ErrorMessage = "É necessário informar ao menos um grupo")]
        public List<int> GruposId { get; set; }

        public long Id { get; set; }

        [Required(ErrorMessage = "É necessário informar o título.")]
        [MinLength(5, ErrorMessage = "O título deve conter no mínimo 5 caracteres.")]
        [MaxLength(30, ErrorMessage = "O título deve conter no máximo 30 caracteres.")]
        public string Titulo { get; set; }
    }
}