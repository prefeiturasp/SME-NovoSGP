using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra
{
    public class RespostaQuestaoDto
    {
        public long Id { get; set; }
        public long? OpcaoRespostaId { get; set; }
        public long QuestaoId { get; set; }
        public Arquivo Arquivo { get; set; }
        public string Texto { get; set; }
        public DateTime? PeriodoInicio { get; set; }
        public DateTime? PeriodoFim { get; set; }
    }
}
