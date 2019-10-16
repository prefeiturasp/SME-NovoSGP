using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Dominio
{
    public enum ModalidadeTipoCalendario
    {
        [Display(Name = "Fundamental/Médio")]
        FundamentalMedio = 1,

        [Display(Name = "EJA")]
        EJA = 2
    }
}
