using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FiltroPesquisaQuestoesPlanoAEEDto
    {
        public FiltroPesquisaQuestoesPlanoAEEDto(long versaoPlanoId, string turmaCodigo)
        {
            VersaoPlanoId = versaoPlanoId;
            TurmaCodigo = turmaCodigo;
        }

        public long VersaoPlanoId { get; set; }
        public string TurmaCodigo { get; set; }
    }
}
