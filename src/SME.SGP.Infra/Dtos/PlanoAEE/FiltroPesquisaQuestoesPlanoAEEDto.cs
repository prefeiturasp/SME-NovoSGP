using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FiltroPesquisaQuestoesPorPlanoAEEIdDto
    {
        public FiltroPesquisaQuestoesPorPlanoAEEIdDto(long? planoAEEId,string turmaCodigo,long codigoAluno)
        {
            PlanoAEEId = planoAEEId;
            TurmaCodigo = turmaCodigo;
            CodigoAluno = codigoAluno;
        }

        public long? PlanoAEEId { get; }
        public long CodigoAluno { get; }
        public string TurmaCodigo { get; set; }
    }
}
