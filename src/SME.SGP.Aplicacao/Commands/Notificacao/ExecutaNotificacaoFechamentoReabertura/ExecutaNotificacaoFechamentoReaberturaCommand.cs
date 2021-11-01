using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExecutaNotificacaoFechamentoReaberturaCommand : IRequest<bool>
    {
        public ExecutaNotificacaoFechamentoReaberturaCommand(FechamentoReabertura fechamentoReabertura, string ue, string dreCodigo)
        {
            FechamentoReabertura = fechamentoReabertura;
            DreCodigo = dreCodigo;
            Ue = ue;
        }

        public FechamentoReabertura FechamentoReabertura { get; set; }
        public string Ue { get; set; }
        public string DreCodigo { get; set; }
    }
}
