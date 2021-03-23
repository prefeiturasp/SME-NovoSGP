using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class QuestaoDto
    {
        public long Id { get; set; }
        public int Ordem { get; set; }
        public string Nome { get; set; }
        public string Observacao { get; set; }
        public bool Obrigatorio { get; set; }
        public bool SomenteLeitura { get; set; }
        public TipoQuestao TipoQuestao { get; set; }
        public string Opcionais { get; set; }
        public OpcaoRespostaDto[] OpcaoResposta { get; set; }
        public IEnumerable<RespostaQuestaoDto> Resposta { get; set; }
    }
}
