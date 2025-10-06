using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio.Enumerados
{
    public enum ComponenteCurricular
    {
        [Display(Name = "LP")]
        Portugues = 1,

        [Display(Name = "MT")]
        Matematica = 2,
            
        [Display(Name = "CN")]
        CienciasNatureza = 3
    }
}
