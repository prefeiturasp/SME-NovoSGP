using Dapper.Contrib.Extensions;
using System;

namespace SME.SGP.Dominio
{
    public class PlanoAEEResposta : EntidadeBase
    {
        public long PlanoAEEQuestaoId { get; set; }
        [Computed]
        public PlanoAEEQuestao PlanoAEEQuestao { get; set; }

        public long? RespostaId { get; set; }
        [Computed]
        public OpcaoResposta Resposta { get; set; }

        public long? ArquivoId { get; set; }
        [Computed]
        public Arquivo Arquivo { get; set; }

        public string Texto { get; set; }
        public DateTime? PeriodoInicio { get; set; }
        public DateTime? PeriodoFim { get; set; }
    }
}
