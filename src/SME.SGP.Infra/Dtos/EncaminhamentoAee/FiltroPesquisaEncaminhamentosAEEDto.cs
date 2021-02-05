using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FiltroPesquisaEncaminhamentosAEEDto
    {
        public long DreId { get; set; }
        public long UeId { get; set; }
        public long TurmaId { get; set; }
        public string AlunoCodigo { get; set; }
        public SituacaoAEE? Situacao { get; set; }
        public string ResponsavelRf { get; set; }
    }
}
