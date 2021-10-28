using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra.Dtos.Fechamento
{
    public class FiltroFechamentoReaberturaNotificacaoDto
    {
        public FiltroFechamentoReaberturaNotificacaoDto(FechamentoReabertura fechamentoReabertura)
        {
            FechamentoReabertura = fechamentoReabertura;
        }

        public FechamentoReabertura FechamentoReabertura{ get; set; }
    }
}
