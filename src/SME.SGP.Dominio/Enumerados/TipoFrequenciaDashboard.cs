using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio.Enumerados
{
    public enum TipoFrequenciaDashboard
    {
        [Display(Name = "Presentes")]
        Presentes = 1,

        [Display(Name = "Remotos")]
        Remotos = 2,

        [Display(Name = "Ausentes")]
        Ausentes = 3,

        [Display(Name = "Total de Estudantes")]
        TotalEstudantes = 4
    }
}
