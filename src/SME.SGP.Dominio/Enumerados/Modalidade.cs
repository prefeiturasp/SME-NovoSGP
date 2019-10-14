using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum Modalidade
    {
        [Display(Name = "Fundamental")]
        Fundamental = 5,

        [Display(Name = "Médio")]
        Medio = 6,

        [Display(Name = "EJA")]
        EJA = 3
    }
}