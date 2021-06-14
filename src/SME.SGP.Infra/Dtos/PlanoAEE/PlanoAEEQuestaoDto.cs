using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class PlanoAEEQuestaoDto
    {
        public long RespostaPlanoId { get; set; }
        public long QuestaoId { get; set; }
        public TipoQuestao TipoQuestao { get; set; } //resposta, texto, arquivo
        public string Resposta { get; set; }

    }
}
