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

        [Display(Name = "Escola Municipal de Educação Infantil", ShortName = "EMEI")]
        EMEI = 2,

        [Display(Name = "Escola Municipal de Ensino Fundamental e Médio", ShortName = "EMEFM")]
        EMEFM = 3,

        [Display(Name = "Escola Municipal de Ensino Bilíngue para Surdos", ShortName = "EMEBS")]
        EMEBS = 4,

        [Display(Name = "Centro Unificado de Educação - Escola Municipal de Ensino Fundamental", ShortName = "CEU EMEF")]
        CEUEMEF = 16,

        [Display(Name = "Centro Unificado de Educação - Escola Municipal de Educação Infantil", ShortName = "CEU EMEI")]
        CEUEMEI = 17,

        [Display(Name = "Centro Municipal de Educação Infantil", ShortName = "CEMEI")]
        CEMEI = 28,

        [Display(Name = "Centro de Educação e Cultura Indígena", ShortName = "CECI")]
        CECI = 30,

        [Display(Name = "Centro Unificado de Educação - Centro Municipal de Educação Infantil", ShortName = "CEU CEMEI")]
        CEUCEMEI = 31
    }
}
