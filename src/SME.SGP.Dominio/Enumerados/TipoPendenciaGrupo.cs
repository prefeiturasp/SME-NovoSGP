﻿using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dominio
{
    public enum TipoPendenciaGrupo
    {
        [Display(Name = "Fechamento")]
        Fechamento = 1,

        [Display(Name = "Calendário")]
        Calendario = 2,

        [Display(Name = "Diario de Classe")]
        DiarioClasse = 3,

        [Display(Name = "AEE")]
        AEE = 4,

        [Display(Name = "Todos")]
        Todos = -99,
    }
}