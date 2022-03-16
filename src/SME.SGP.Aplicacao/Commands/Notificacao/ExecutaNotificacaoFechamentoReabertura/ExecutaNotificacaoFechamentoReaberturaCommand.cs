using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExecutaNotificacaoFechamentoReaberturaCommand : IRequest<bool>
    {
        public ExecutaNotificacaoFechamentoReaberturaCommand(FiltroFechamentoReaberturaNotificacaoDto fechamentoReabertura)
        {
            FechamentoReabertura = fechamentoReabertura;
        }

        public FiltroFechamentoReaberturaNotificacaoDto FechamentoReabertura { get; set; }
    }
}
