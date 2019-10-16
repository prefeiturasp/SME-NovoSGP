using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Dominio.Enumerados
{
    public enum EventoLocalOcorrencia
    {
        [Display(Name = "Unidade Escolar")]
        UE = 1,
        [Display(Name = "Diretoria Municipal de Educação")]
        DRE,
        [Display(Name = "Secretaria Municipal de Educação")]
        SME,
        [Display(Name = "Secretaria Municipal de Educação / Unidade Escolar")]
        SMEUE,
        [Display(Name = "Todos")]
        Todos
    }
}
