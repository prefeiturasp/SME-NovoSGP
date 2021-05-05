using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Dominio
{
    public enum StatusFechamento
    {
        [Display(Description =  "Não Iniciado")]
        NaoIniciado = 1,

        [Display(Description = "Em Andamento")]
        EmAndamento = 2,

        [Display(Description = "Concluído")]
        Concluido = 3, 
    }
}
