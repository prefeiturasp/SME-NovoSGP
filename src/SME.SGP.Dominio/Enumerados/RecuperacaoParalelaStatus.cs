using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum RecuperacaoParalelaStatus
    {
        [Display(Name = "Não alterado")]
        NaoAlterado = 0,

        [Display(Name = "Alerta")]
        Alerta = 1,

        [Display(Name = "Concluído")]
        Concluido = 2
    }
}