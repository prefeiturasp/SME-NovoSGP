using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum TipoAlteracaoNota
    {
        [Display(Name = "Ambas")]
        Ambas = 1,

        [Display(Name = "Fechamento")]
        Fechamento = 2,

        [Display(Name = "Conselho de Classe")]
        ConselhoClasse = 3
    }
}
