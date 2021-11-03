using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FiltroNotificacaoFechamentoReaberturaUEDto
    {
        public FiltroNotificacaoFechamentoReaberturaUEDto(FiltroFechamentoReaberturaNotificacaoDto fechamentoReabertura)
        {
            FechamentoReabertura = fechamentoReabertura;
        }

        public FiltroFechamentoReaberturaNotificacaoDto FechamentoReabertura { get; set; }
    }
}
