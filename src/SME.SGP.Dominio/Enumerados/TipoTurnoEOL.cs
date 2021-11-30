using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Dominio
{
    public enum TipoTurnoEOL
    {
        [Display(Name = "Manhã")]
        Manha = 1,
        [Display(Name = "Intermediário")]
        Intermediario = 2,
        [Display(Name = "Tarde")]
        Tarde = 3,
        [Display(Name = "Vespertino")]
        Vespertino = 3,
        [Display(Name = "Noite")]
        Noite = 3,
        [Display(Name = "Integral")]
        Integral = 3,
    }
}
