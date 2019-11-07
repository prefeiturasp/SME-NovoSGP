using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum FeriadoEnum
    {
        [Display(Name = "Carnaval")]
        Carnaval = 1,

        [Display(Name = "Sexta-feira Santa")]
        SextaSanta = 3,

        [Display(Name = "Corpus Christi")]
        CorpusChristi = 4,

        [Display(Name = "Páscoa")]
        Pascoa = 5
    }
}