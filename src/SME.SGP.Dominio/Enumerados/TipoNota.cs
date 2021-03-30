using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum TipoNota
    {
        [Display(Name = "Todas")]
        Todas = 0,
        [Display(Name = "Nota")]
        Nota = 1,
        [Display(Name = "Conceito")]
        Conceito = 2,
        [Display(Name = "Sintese")]
        Sintese = 3
    }
}
