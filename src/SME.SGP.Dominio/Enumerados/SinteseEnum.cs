using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Dominio
{
    public enum SinteseEnum
    {
        [Display(Name = "Frequente")]
        Frequente = 1,

        [Display(Name = "Não frequente")]
        NaoFrequente = 2
    }
}
