using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio.Enumerados
{
    public enum ComponenteCurricularEnum : short
    {
        [Display(Name = "LP")]
        Portugues = 138,

        [Display(Name = "MT")]
        Matematica = 2,
            
        [Display(Name = "CN")]
        CienciasNatureza = 89
    }
}
