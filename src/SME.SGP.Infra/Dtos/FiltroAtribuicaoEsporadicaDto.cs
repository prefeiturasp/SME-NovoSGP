using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Infra
{
    public class FiltroAtribuicaoEsporadicaDto
    {
        [Required(ErrorMessage = "É necessario informar o Id da DRE")]
        public string DreId { get; set; }
        [Required(ErrorMessage = "É necessario informar o Id da UE")]
        public string UeId { get; set; }
        public string CodigoRF { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "É necessario informar o ano letivo")]
        public int AnoLetivo { get; set; }
    }
}
