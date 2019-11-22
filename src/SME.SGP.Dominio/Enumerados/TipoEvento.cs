using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum TipoEvento
    {
        [Display(Name = "Liberação Excepcional")]
        LiberacaoExcepcional = 6,

        [Display(Name = "Organização Escolar")]
        OrganizacaoEscolar = 8,

        [Display(Name = "Recesso")]
        Recesso = 11,

        [Display(Name = "Reposição no recesso")]
        ReposicaoNoRecesso = 15,
    }
}