using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Dominio
{
    public enum TipoNota
    {
        [Display(Name = "Nota")]
        Nota = 1,
        [Display(Name = "Conceito")]
        Conceito = 2,
        [Display(Name = "Conceito")]
        Sintese = 3
    }
}
