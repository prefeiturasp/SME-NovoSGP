using System;
using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos
{
    public class FiltroNotificacaoAlunosFreqMensalInsuficienteBuscaAtiva
    {
        public FiltroNotificacaoAlunosFreqMensalInsuficienteBuscaAtiva() { }
        public IEnumerable<ConsolidacaoFreqAlunoMensalInsuficienteDto> ConsolidacoesFrequenciaMensalInsuficientes { get; set; }
        public IEnumerable<AtribuicaoResponsavelDto> ResponsaveisNotificacao { get; set; }
    }
}
