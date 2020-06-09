using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Infra.Enumerados
{
    public enum TipoRelatorio
    {
        [Display(Name = "relatorio/conselhoclassealuno")]
        ConselhoClasseAluno = 1,

        [Display(Name = "relatorio/conselhoclasseturma")]
        ConselhoClasseTurma = 2
    }
}
