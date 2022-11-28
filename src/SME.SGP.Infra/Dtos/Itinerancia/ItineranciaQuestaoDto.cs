using System;
using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class ItineranciaQuestaoDto
    {
        public long Id { get; set; }
        public long QuestaoId { get; set; }

        public string Descricao { get; set; }
        public string Resposta { get; set; }
        public long ItineranciaId { get; set; }

        public long? ArquivoId { get; set; }
        public string ArquivoNome { get; set; }
        public Guid ArquivoCodigo { get; set; }
        public TipoQuestao TipoQuestao { get; set; }
        public bool? Obrigatorio { get; set; }
        public bool Excluido { get; set; } = false;
    }
}