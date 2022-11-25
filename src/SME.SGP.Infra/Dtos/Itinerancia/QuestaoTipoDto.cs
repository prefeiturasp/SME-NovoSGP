using System;
using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class QuestaoTipoDto
    {
        public long QuestaoId { get; set; }
        public long? ArquivoId { get; set; }
        public string ArquivoNome { get; set; }
        public Guid ArquivoCodigo { get; set; }
        public TipoQuestao TipoQuestao { get; set; }
    }
}