using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FiltroPesquisaQuestoesPorPlanoAEEIdDto
    {
        public FiltroPesquisaQuestoesPorPlanoAEEIdDto(long? planoAEEId, string turmaCodigo)
        {
            PlanoAEEId = planoAEEId;
            TurmaCodigo = turmaCodigo;
        }

        public long? PlanoAEEId { get; }
        public string TurmaCodigo { get; set; }
    }
}
