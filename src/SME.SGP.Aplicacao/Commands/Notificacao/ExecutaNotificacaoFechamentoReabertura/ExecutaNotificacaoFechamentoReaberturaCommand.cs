using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExecutaNotificacaoFechamentoReaberturaCommand : IRequest<bool>
    {
        public ExecutaNotificacaoFechamentoReaberturaCommand(FechamentoReabertura fechamentoReabertura, IEnumerable<string> ues, string dreCodigo)
        {
            FechamentoReabertura = fechamentoReabertura;
            DreCodigo = dreCodigo;
            Ues = ues;
        }

        public FechamentoReabertura FechamentoReabertura { get; set; }
        public IEnumerable<string> Ues { get; set; }
        public string DreCodigo { get; set; }
    }
}
