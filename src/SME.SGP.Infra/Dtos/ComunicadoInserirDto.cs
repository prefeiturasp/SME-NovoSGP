using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dto
{
    public class ComunicadoInserirDto
    {
        [DataRequerida(ErrorMessage = "A data de envio é obrigatória.")]
        [DataMaiorAtual(ErrorMessage = "A data de envio deve ser igual ou maior que a data atual.")]
        public DateTime DataEnvio { get; set; }

        [DataMaiorAtual(ErrorMessage = "A data de expiração deve ser igual ou maior que a data atual.")]
        public DateTime? DataExpiracao { get; set; }

        [Required(ErrorMessage = "É necessário informar a descrição.")]
        [MinLength(5, ErrorMessage = "A descrição deve conter no mínimo 5 caracteres.")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O grupo do comunicado deve ser informado.")]
        [ListaTemElementos(ErrorMessage = "É necessário informar ao menos um grupo")]
        public List<int> GruposId { get; set; }

        public long Id { get; set; }

        [Required(ErrorMessage = "É necessário informar o título.")]
        [MinLength(10, ErrorMessage = "O título deve conter no mínimo 10 caracteres.")]
        [MaxLength(50, ErrorMessage = "O título deve conter no máximo 50 caracteres.")]
        public string Titulo { get; set; }
    }
}