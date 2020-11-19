using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Dominio.Enumerados
{
    public enum TipoDocumento
    {
        [Display(Name = "Plano de Trabalho")]
        PlanoTrabalho = 1,

        [Display(Name = "Documento")]
        Documento = 2
    }
}
