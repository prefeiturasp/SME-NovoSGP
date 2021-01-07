using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Dominio.Enumerados
{
    public enum SituacaoAEE
    {
        [Display(Name = "Rascunho")]
        rascunho = 1,
        [Display(Name = "Encaminhado")]
        encaminhado = 2,
        [Display(Name = "Finalizado")]
        finalizado = 3,
    }
}
