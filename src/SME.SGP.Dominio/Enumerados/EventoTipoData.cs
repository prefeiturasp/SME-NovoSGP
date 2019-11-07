using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum EventoTipoData
    {
        [Display(Name = "Evento único")]
        Unico = 1,

        [Display(Name = "Evento com período definido")]
        InicioFim = 2
    }
}