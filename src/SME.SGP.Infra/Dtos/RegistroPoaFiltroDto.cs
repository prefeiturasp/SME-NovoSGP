using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Infra
{
    public class RegistroPoaFiltroDto
    {
        [Required(ErrorMessage = "O código rf é obrigatório")]
        public string CodigoRf { get; set; }
        [Required(ErrorMessage = "É obrigatório informar o id da Dre")]
        public string DreId { get; set; }
        [Required(ErrorMessage = "É obrigatório informar o Mes")]
        [Range(1,12, ErrorMessage = "Os meses devem ser infomados entre Janeiro e Dezembro")]
        public int Mes { get; set; }
        [Required(ErrorMessage = "E necessário informar o id da unidade escolar")]
        public string UeId { get; set; }
        public string Titulo { get; set; }
    }
}
