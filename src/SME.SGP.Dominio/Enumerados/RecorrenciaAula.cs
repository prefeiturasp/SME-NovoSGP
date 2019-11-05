using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum RecorrenciaAula
    {
        [Display(Name = "Aula única")]
        AulaUnica = 1,

        [Display(Name = "Repetir no bimestre atual")]
        RepetirBimestreAtual = 2,

        [Display(Name = "Repetir em todos os bimestres")]
        RepetirTodosBimestres = 3
    }
}
