using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum Modalidade
    {
        [Display(Name = "Fundamental")]
        Fundamental = 1,
        
        [Display(Name = "Médio")]
        Medio = 2,

        [Display(Name = "EJA")]
        EJA = 3
    }
}
