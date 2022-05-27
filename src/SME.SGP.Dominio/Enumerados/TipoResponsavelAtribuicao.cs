using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum TipoResponsavelAtribuicao
    {
        [Display(Name = "Supervisor Escolar")]
        SupervisorEscolar = 1,

        [Display(Name = "PAAI")]
        PAAI = 2,

        [Display(Name = "Psicólogo Escolar")]
        PsicologoEscolar = 3,

        [Display(Name = "Psicopedagogo")]
        Psicopedagogo = 4,

        [Display(Name = "Assistente Social")]
        AssistenteSocial = 5,
    }
}
