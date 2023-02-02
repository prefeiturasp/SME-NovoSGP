using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum FuncaoExterna
    {
        [Display(Name = "Diretor")]
        Diretor = 1,

        [Display(Name = "Coordenador Pedagógico")]
        CP = 2,

        [Display(Name = "Diretor de Escola")]
        ProfessorEI = 3,

        [Display(Name = "Assistente de Diretor")]
        AD = 7,

        [Display(Name = "Auxiliar de Secretaria")]
        ATE = 18,

        [Display(Name = "Secretário Escolar")]
        Secretario = 33
    }
}