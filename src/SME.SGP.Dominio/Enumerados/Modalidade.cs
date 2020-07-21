using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum Modalidade
    {
        [Display(Name = "Infantil", ShortName = "EI")]
        Infantil = 1,

        [Display(Name = "Fundamental", ShortName = "EF")]
        Fundamental = 5,

        [Display(Name = "Médio", ShortName = "EM")]
        Medio = 6,

        [Display(Name = "EJA", ShortName = "EJA")]
        EJA = 3
    }
}