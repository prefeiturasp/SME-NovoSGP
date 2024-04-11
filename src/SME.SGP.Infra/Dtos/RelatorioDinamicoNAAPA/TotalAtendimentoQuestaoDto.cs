using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class TotalAtendimentoQuestaoDto
    {
        public string Descricao { get; set; }
        public List<TotalDeAtendimentoQuestaoPorRespostaDto> TotalAtendimentoQuestaoPorRespostas { get; set; }
    }
}
