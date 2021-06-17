using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum AtaFinalTipoVisualizacao
    {
        [Display(Name = "Turma")]
        Turma = 1,

        [Display(Name = "Estudantes")]
        Estudantes = 2,
    }
}
