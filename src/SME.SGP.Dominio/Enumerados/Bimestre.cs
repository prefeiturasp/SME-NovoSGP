using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Dominio
{
    public enum Bimestre
    {
        [Display(Name = "Todos")]
        Todos = -99,
        [Display(Name = "1°")]
        Primeiro = 1,
        [Display(Name = "2°")]
        Segundo = 2,
        [Display(Name = "3°")]
        Terceiro = 3,
        [Display(Name = "4°")]
        Quarto = 4,
        [Display(Name = "Final")]
        Final = 0
    }
}