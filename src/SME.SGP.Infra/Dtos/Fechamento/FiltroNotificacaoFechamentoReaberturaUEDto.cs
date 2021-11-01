using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FiltroNotificacaoFechamentoReaberturaUEDto
    {
        public FiltroNotificacaoFechamentoReaberturaUEDto(string dre, string ue, FechamentoReabertura fechamentoReabertura)
        {
            Dre = dre;
            Ue = ue;
            FechamentoReabertura = fechamentoReabertura;
        }

        public string Dre { get; set; }
        public string Ue { get; set; }
        public FechamentoReabertura FechamentoReabertura{ get; set; }
    }
}
