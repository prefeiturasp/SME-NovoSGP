using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class PendenciaFechamentoResumoDto
    {
        public long PendenciaId { get; set; }
        public long DisciplinaId { get; set; }
        public string ComponenteCurricular { get; set; }
        public string Descricao { get; set; }
        public int Situacao { get; set; }
        public string SituacaoNome { get; set; }
    }
}
