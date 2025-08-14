using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Dominio
{
    public enum FuncaoAtividade
    {
        [Display(Name = "Coord. Pedagógico CIEJA")]
        COORDERNADOR_PEDAGOGICO_CIEJA = 44,

        [Display(Name = "Assistente Coord. Geral CIEJA")]
        ASSISTENTE_COORDERNADOR_GERAL_CIEJA = 43,

        [Display(Name = "Coord. Geral CIEJA")]
        COORDERNADOR_GERAL_CIEJA = 42
    }
}
