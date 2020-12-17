using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Dominio
{
    public enum TipoConteudoArquivo
    {
        [Display(Name = "")]
        Indefinido = 0,

        [Display(Name = "application/pdf")]
        PDF = 1,
    }
}
