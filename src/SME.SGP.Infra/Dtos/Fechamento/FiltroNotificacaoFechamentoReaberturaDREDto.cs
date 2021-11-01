using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FiltroNotificacaoFechamentoReaberturaDREDto
    {
        public FiltroNotificacaoFechamentoReaberturaDREDto(string dre, IEnumerable<string> ues, FechamentoReabertura fechamentoReabertura)
        {
            Dre = dre;
            Ues = ues;
            FechamentoReabertura = fechamentoReabertura;
        }

        public string Dre { get; set; }
        public IEnumerable<string> Ues { get; set; }
        public FechamentoReabertura FechamentoReabertura{ get; set; }
    }
}
