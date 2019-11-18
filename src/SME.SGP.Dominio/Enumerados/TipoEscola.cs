using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Dominio
{
    public enum TipoEscola
    {
        [Display(Name = "Não Informado", ShortName = "NA")]
        Nenhum = 0,

        [Display(Name = "Escola Municipal de Ensino Fundamental", ShortName = "EMEF")]
        EMEF = 1,

        [Display(Name = "Escola Municipal de Ensino Fundamental e Médio", ShortName = "EMEFM")]
        EMEFM = 3,

        [Display(Name = "Escola Municipal de Ensino Bilíngue para Surdos", ShortName = "EMEBS")]
        EMEBS = 4,

        [Display(Name = "Centro Unificado de Educação - Escola Municipal de Ensino Fundamental", ShortName = "CEU EMEF")]
        CEUEMEF = 16
    }
}
