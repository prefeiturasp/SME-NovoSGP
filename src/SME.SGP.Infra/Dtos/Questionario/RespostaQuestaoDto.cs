using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;

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

    public static class RespostaQuestaoDtoExtensao
    {
        public static bool NaoNuloEContemRegistrosRespondidos(this IEnumerable<RespostaQuestaoDto> respostas)
        {
            return respostas.NaoEhNulo() && respostas.Any(resposta => !string.IsNullOrEmpty(resposta.Texto) || (resposta.OpcaoRespostaId ?? 0) != 0);
        }
    }
}
