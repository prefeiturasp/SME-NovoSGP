using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Dominio
{
    public enum TipoValor
    {
        [Display(Name = "Numerico")]
        Numerico = 1,
        [Display(Name = "Textual")]
        Textual = 2
    }
}
