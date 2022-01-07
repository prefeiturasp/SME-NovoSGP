using System;
using System.Collections.Generic;
using System.Text;
using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class FiltroPesquisaQuestoesPorPlanoAEEIdDto
    {
        public FiltroPesquisaQuestoesPorPlanoAEEIdDto(long? planoAEEId, string turmaCodigo, bool historico = false)
        {
            PlanoAEEId = planoAEEId;
            TurmaCodigo = turmaCodigo;
            Historico = historico;
        }

        public long? PlanoAEEId { get; }
        public string TurmaCodigo { get; set; }
        public bool Historico { get; set; } = false;
    }
}
