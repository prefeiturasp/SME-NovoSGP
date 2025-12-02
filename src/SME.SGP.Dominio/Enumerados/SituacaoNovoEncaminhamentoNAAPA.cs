using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SME.SGP.Dominio.Enumerados
{
    public enum SituacaoNovoEncaminhamentoNAAPA
    {
        [Display(Name = "Aguardando atendimento")]
        AguardandoAtendimento = 2,
        [Display(Name = "Em atendimento")]
        EmAtendimento = 3,
        [Display(Name = "Encerrado")]
        Encerrado = 4
    }
}