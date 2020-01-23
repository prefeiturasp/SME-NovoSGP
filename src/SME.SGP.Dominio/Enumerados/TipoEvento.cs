using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum TipoEvento
    {
        [Display(Name = "Feriado")]
        Feriado = 4,

        [Display(Name = "Liberação Excepcional")]
        LiberacaoExcepcional = 6,

        [Display(Name = "Organização Escolar")]
        OrganizacaoEscolar = 8,

        [Display(Name = "Outros")]
        Outros = 9,

        [Display(Name = "Recesso")]
        Recesso = 11,

        [Display(Name = "Recreio nas Férias")]
        RecreioNasFerias = 12,

        [Display(Name = "Reposição de Aula")]
        ReposicaoDeAula = 13,

        [Display(Name = "Reposição do Dia")]
        ReposicaoDoDia = 14,

        [Display(Name = "Reposição no recesso")]
        ReposicaoNoRecesso = 15,

        [Display(Name = "Suspensão de Atividades")]
        SuspensaoAtividades = 21,
    }
}