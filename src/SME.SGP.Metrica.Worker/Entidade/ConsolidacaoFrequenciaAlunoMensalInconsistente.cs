using System;

namespace SME.SGP.Metrica.Worker.Entidade
{
    public class ConsolidacaoFrequenciaAlunoMensalInconsistente
    {
        public ConsolidacaoFrequenciaAlunoMensalInconsistente()
        {
            Data = DateTime.Now.Date.ToUniversalTime();
        }
        public long TurmaId { get; set; }
        public string AlunoCodigo { get; set; }
        public int Mes { get; set; }
        public int QuantidadeAulas { get; set; }
        public int QuantidadeAusencias { get; set; }
        public int QuantidadeCompensacoes { get; set; }
        public int QuantidadeAulasCalculado { get; set; }
        public int QuantidadeAusenciasCalculado { get; set; }
        public int QuantidadeCompensacoesCalculado { get; set; }
        public DateTime Data { get; set; }

    }
}
