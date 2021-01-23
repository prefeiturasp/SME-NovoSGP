using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Dominio.Enumerados
{
    public enum SituacaoAEE
    {
        [Display(Name = "Rascunho")]
        Rascunho = 1,
        [Display(Name = "Encaminhado")]
        Encaminhado = 2,
        [Display(Name = "Finalizado")]
        Finalizado = 3,
    }
}
