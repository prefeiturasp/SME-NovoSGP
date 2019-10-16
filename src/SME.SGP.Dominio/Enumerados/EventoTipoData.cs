using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Dominio.Enumerados
{
    public enum EventoTipoData
    {
        [Display(Name = "Evento unico")]
        Unico = 1,
        [Display(Name = "Evento com período definido")]
        InicioFim
    }
}
