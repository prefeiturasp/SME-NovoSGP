using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FiltroPesquisaQuestoesPlanoAEEDto
    {
        public FiltroPesquisaQuestoesPlanoAEEDto(long questionarioId, long versaoPlanoId, string turmaCodigo)
        {
            QuestionarioId = questionarioId;
            VersaoPlanoId = versaoPlanoId;
            TurmaCodigo = turmaCodigo;
        }

        public long QuestionarioId { get; }
        public long VersaoPlanoId { get; set; }
        public string TurmaCodigo { get; set; }
    }
}
